package html

import (
	"context"
	"fmt"
	"strconv"
	"strings"

	"github.com/PuerkitoBio/goquery"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"github.com/gocolly/colly/v2"
	log "github.com/sirupsen/logrus"
)

type XboxStoreHtmlParser struct {
	XboxStoreUrl string
}

// def __parse_old_price(self, price_placement: Any, currency: str) -> float | None:
// price = price_placement.find("s")
// if price == "" or price is None:
// 	return None
// return self.__try_parse_float(price.text.replace(currency, "").replace(",", ".").strip())

func parseOldPrice(price_placement *goquery.Selection, currency string) (*float64, error) {
	price := price_placement.Find("s")
	if price.Length() == 0 {
		return nil, fmt.Errorf("oldprice is empty")
	}
	priceWithoutCurrency := strings.Replace(price.Text(), currency, "", 1)
	priceStr := strings.Replace(priceWithoutCurrency, ",", ".", 1)
	priceFloat, err := strconv.ParseFloat(strings.TrimSpace(priceStr), 64)
	if err != nil {
		return nil, err
	}
	return &priceFloat, nil
}

func parsePrice(selection *goquery.Selection) (*float64, error) {
	price := selection.Find("span[itemprop=price]").AttrOr("content", "")
	if price == "" {
		return nil, fmt.Errorf("price is empty")
	}
	priceFloat, err := strconv.ParseFloat(strings.Replace(price, ",", ".", 1), 64)
	if err != nil {
		return nil, fmt.Errorf("price is not float")
	}
	return &priceFloat, nil
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
			result <- data.NewXboxStoreGame(title, strings.Replace(link, "/p/", "/games/store/", 1), price, oldPrice)

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
