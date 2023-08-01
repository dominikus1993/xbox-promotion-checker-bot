package html

import (
	"context"
	"fmt"
	"regexp"
	"strconv"
	"strings"

	"github.com/PuerkitoBio/goquery"
	gotolkit "github.com/dominikus1993/go-toolkit/channels"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"github.com/gocolly/colly/v2"
	log "github.com/sirupsen/logrus"
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

func parsePriceFromSelection(selection *goquery.Selection, currency string) (float64, error) {
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
	regularPrice, err = parsePriceFromSelection(regularPriceE, polishCurrency)
	if err != nil {
		return
	}

	promotionPriceE := element.Find("span.font-weight-semibold")
	promotionPrice, err = parsePriceFromSelection(promotionPriceE, polishCurrency)
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
				log.WithField("url", e.Request.URL).WithField("link", link).WithField("title", title).Warnln("Is In GamePass or EaAccess")
				return
			}
			oldPrice, promotionPrice, err := parsePrices(price_placement)
			if err != nil {
				log.WithField("url", e.Request.URL).WithField("link", link).WithField("title", title).WithError(err).Warnln("failed to parse price")
				return
			}
			newGame := data.NewXboxStoreGame(title, link, promotionPrice, oldPrice)
			if newGame.IsValidGame() {
				result <- newGame
			} else {
				log.WithField("Url", e.Request.URL).Warning("Can't parse game because is invalid")
			}
		})
		parser.collector.OnError(func(r *colly.Response, err error) {
			log.WithError(err).WithField("page", page).Errorln("Error while parsing page")
		})
		err := parser.collector.Visit(pageUrl)
		if err != nil {
			log.WithError(err).WithField("page", page).Errorln("Error while parsing page")
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
