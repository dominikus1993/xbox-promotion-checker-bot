package html

import (
	"context"
	"fmt"

	"github.com/PuerkitoBio/goquery"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"github.com/gocolly/colly/v2"
	log "github.com/sirupsen/logrus"
)

type XboxStoreHtmlParser struct {
	XboxStoreUrl string
}

func parsePrice(selection *goquery.Selection) string {
	return selection.Text()
}

func (parser *XboxStoreHtmlParser) getXboxPageUrl(page int) string {
	if page == 1 {
		return parser.XboxStoreUrl
	}
	skipItems := (page - 1) * 90
	return fmt.Sprintf("%s?s=store&skipitems=%d", parser.XboxStoreUrl, skipItems)
}

func (parser *XboxStoreHtmlParser) parsePage(ctx context.Context, page int) <-chan data.XboxStoreGame {
	result := make(chan data.XboxStoreGame)
	pageUrl := parser.getXboxPageUrl(page)
	go func() {
		c := colly.NewCollector()
		c.OnHTML("div .m-channel-placement-item", func(e *colly.HTMLElement) {
			product_placement := e.DOM.Find("div .m-channel-placement-item")
			price_placement := product_placement.Find("div .c-channel-placement-price")
			prices := price_placement.Find("div .c-price")

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

func (parser *XboxStoreHtmlParser) Parse(ctx context.Context) <-chan data.XboxStoreGame {
	result := make(chan data.XboxStoreGame)
	go func() {

	}()
	return result
}
