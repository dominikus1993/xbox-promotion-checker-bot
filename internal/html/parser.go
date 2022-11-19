package html

import (
	"context"
	"fmt"
	"strconv"
	"strings"

	"github.com/PuerkitoBio/goquery"
	gotolkit "github.com/dominikus1993/go-toolkit"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"github.com/gocolly/colly/v2"
	log "github.com/sirupsen/logrus"
)

type XboxStoreHtmlParser struct {
	xboxStoreUrl string
}

func NewXboxStoreHtmlParser(xboxStoreUrl string) *XboxStoreHtmlParser {
	return &XboxStoreHtmlParser{xboxStoreUrl: xboxStoreUrl}
}

func parseOldPrice(price_placement *goquery.Selection, currency string) (float64, error) {
	price := price_placement.Find("s")
	if price.Length() == 0 {
		return 0.0, fmt.Errorf("oldprice is empty")
	}
	priceWithoutCurrency := strings.Replace(price.Text(), currency, "", 1)
	priceStr := strings.Replace(priceWithoutCurrency, ",", ".", 1)
	priceFloat, err := strconv.ParseFloat(strings.TrimSpace(priceStr), 64)
	if err != nil {
		return 0.0, err
	}
	return priceFloat, nil
}

func parsePrice(selection *goquery.Selection) (float64, error) {
	price := selection.Find("span[itemprop=price]").AttrOr("content", "")
	if price == "" {
		return 0.0, fmt.Errorf("price is empty")
	}
	priceFloat, err := strconv.ParseFloat(strings.Replace(price, ",", ".", 1), 64)
	if err != nil {
		return 0.0, fmt.Errorf("price is not float")
	}
	return priceFloat, nil
}

func (parser *XboxStoreHtmlParser) getXboxPageUrl(page int) string {
	if page == 1 {
		return parser.xboxStoreUrl
	}
	skipItems := (page - 1) * 90
	return fmt.Sprintf("%s?skipitems=%d", parser.xboxStoreUrl, skipItems)
}

func (parser *XboxStoreHtmlParser) parsePage(ctx context.Context, page int) <-chan data.XboxStoreGame {
	result := make(chan data.XboxStoreGame)
	pageUrl := parser.getXboxPageUrl(page)
	go func() {
		c := colly.NewCollector()
		c.OnHTML("div .m-channel-placement-item", func(e *colly.HTMLElement) {
			product_placement := e.DOM.Find("div .c-channel-placement-content")
			price_placement := product_placement.Find("div .c-channel-placement-price")
			prices := price_placement.Find("div .c-price")
			price, err := parsePrice(prices)
			if err != nil {
				return
			}
			oldPrice, err := parseOldPrice(price_placement, "zł")
			if err != nil {
				return
			}
			link := e.DOM.Find("a").AttrOr("href", "")
			title := product_placement.Find("div .c-subheading-6").Text()
			result <- data.NewXboxStoreGame(title, link, price, oldPrice)

		})
		c.OnError(func(r *colly.Response, err error) {
			log.WithError(err).WithField("page", page).Errorln("Error while parsing page")
		})
		err := c.Visit(pageUrl)
		if err != nil {
			log.WithError(err).WithField("page", page).Errorln("Error while parsing page")
		}
		close(result)
	}()
	return result
}

func (parser *XboxStoreHtmlParser) Provide(ctx context.Context) <-chan data.XboxStoreGame {
	streams := make([]<-chan data.XboxStoreGame, 0)
	for i := 1; i <= 7; i++ {
		streams = append(streams, parser.parsePage(ctx, i))
	}
	return gotolkit.FanIn(ctx, streams...)
}
