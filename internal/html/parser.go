package html

import (
	"context"
	"fmt"
	"regexp"
	"strconv"
	"strings"

	"github.com/PuerkitoBio/goquery"
	gotolkit "github.com/dominikus1993/go-toolkit/channels"
	data "github.com/dominikus1993/xbox-promotion-checker-bot/pkg/model"
	"github.com/gocolly/colly/v2"
	"golang.org/x/exp/slog"
)

const polishCurrency = "zł"

var decimalRegExp = regexp.MustCompile(`[^0-9.,-]`)
var emptySpace = []byte("")
var commaRegExp = regexp.MustCompile(`,`)

type XboxStoreHtmlParser struct {
	xboxStoreUrl string
	collector    *colly.Collector
}

func NewXboxStoreHtmlParser(xboxStoreUrl string, collector *colly.Collector) *XboxStoreHtmlParser {
	return &XboxStoreHtmlParser{xboxStoreUrl: xboxStoreUrl, collector: collector}
}

func parsePrice(txt string) (float64, error) {
	if len(txt) == 0 {
		return 0.0, fmt.Errorf("price is empty")
	}
	// Zamiana przecinka na kropkę
	txt = string(commaRegExp.ReplaceAll([]byte(txt), []byte(".")))
	// Usunięcie wszystkich znaków, które nie są cyframi, kropką ani minusem
	priceStr := string(decimalRegExp.ReplaceAll([]byte(txt), emptySpace))
	priceFloat, err := strconv.ParseFloat(strings.TrimSpace(priceStr), 64)
	if err != nil {
		return 0.0, err
	}
	return priceFloat, nil
}

func parsePriceFromSelection(selection *goquery.Selection) (float64, error) {
	price := selection.Text()
	return parsePrice(price)
}

func (parser *XboxStoreHtmlParser) getXboxPageUrl(page int) string {
	if page == 1 {
		return parser.xboxStoreUrl
	}
	skipItems := (page - 1) * 90
	return fmt.Sprintf("%s?skipitems=%d", parser.xboxStoreUrl, skipItems)
}

func parsePrices(element *goquery.Selection) (regularPrice data.RegularPrice, promotionPrice data.PromotionPrice, err error) {
	regularPriceE := element.Find("span.text-line-through")
	regularPrice, err = parsePriceFromSelection(regularPriceE)
	if err != nil {
		return
	}

	promotionPriceE := element.Find("span.font-weight-semibold")
	promotionPrice, err = parsePriceFromSelection(promotionPriceE)
	return
}

func isIngGamePassOrEaAccess(element *goquery.Selection) bool {
	return len(element.Find("span.text-line-through").Text()) == 0
}

func getTitleAndLink(element *goquery.Selection) (title data.Title, link data.Link) {
	el := element.Find("a")
	link = el.AttrOr("href", "")
	title = el.Text()
	return
}

func (parser *XboxStoreHtmlParser) parsePage(ctx context.Context, page int) <-chan data.XboxStoreGame {
	result := make(chan data.XboxStoreGame)
	pageUrl := parser.getXboxPageUrl(page)
	go func() {
		defer close(result)
		parser.collector.OnHTML("div.card", func(e *colly.HTMLElement) {
			card_placement := e.DOM.Find("div.card-body")
			price_placement := card_placement.Find("p[aria-hidden='true']")
			title, link := getTitleAndLink(card_placement)
			if isIngGamePassOrEaAccess(price_placement) {
				slog.WarnContext(ctx, "is in GamePass or EaAccess", slog.String("link", link), slog.String("title", title), slog.String("url", e.Request.URL.String()))
				return
			}
			oldPrice, promotionPrice, err := parsePrices(price_placement)
			if err != nil {
				slog.WarnContext(ctx, "failed to parse price", slog.String("link", link), slog.String("title", title), slog.String("url", e.Request.URL.String()))
				return
			}
			newGame := data.NewXboxStoreGame(title, link, promotionPrice, oldPrice)
			if newGame.IsValidGame() {
				result <- newGame
			} else {
				slog.WarnContext(ctx, "can't parse game because is invalid", slog.String("link", link), slog.String("title", title), slog.String("url", e.Request.URL.String()))
			}
		})
		parser.collector.OnError(func(r *colly.Response, err error) {
			slog.ErrorContext(ctx, "error while parsing page", "error", err, "page", page)
		})
		err := parser.collector.Visit(pageUrl)
		if err != nil {
			slog.ErrorContext(ctx, "error while parsing page", "error", err, "page", page)
		}
		parser.collector.Wait()
	}()
	return result
}

func (parser *XboxStoreHtmlParser) Provide(ctx context.Context) <-chan data.XboxStoreGame {
	const pages = 10
	streams := make([]<-chan data.XboxStoreGame, pages)
	for i := 0; i < pages; i++ {
		streams[i] = parser.parsePage(ctx, i+1)
	}
	return gotolkit.FanIn(ctx, streams...)
}
