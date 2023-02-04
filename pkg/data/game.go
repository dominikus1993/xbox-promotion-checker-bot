package data

import (
	"net/url"
	"strings"

	"github.com/dustin/go-humanize"
)

const baseMicrosoftPath = "https://www.microsoft.com/"

type PromotionPrice = float64

type RegularPrice = float64

type Title = string
type Link = string

type XboxStoreGame struct {
	Title    Title
	link     Link
	price    PromotionPrice
	oldPrice RegularPrice
}

func joinPath(uri string) (string, error) {
	if strings.HasPrefix(uri, baseMicrosoftPath) {
		return uri, nil
	}
	return url.JoinPath("https://www.microsoft.com/", uri)
}

func (g *XboxStoreGame) GetLink() (*url.URL, error) {

	uri, err := joinPath(g.link)
	if err != nil {
		return nil, err
	}

	u, err := url.ParseRequestURI(uri)
	if err != nil {
		return nil, err
	}
	return u, nil
}

func (g *XboxStoreGame) CalculatePromotionPercentage() float64 {
	return 100 - (g.price / g.oldPrice * 100)
}

func (g *XboxStoreGame) FormatPromotionPercentage() string {
	percentage := 100 - (g.price / g.oldPrice * 100)
	return humanize.FormatFloat("#,###.##", percentage)
}

func (g *XboxStoreGame) GetPrice() string {
	return humanize.FormatFloat("#,###.##", g.price)
}

func (g *XboxStoreGame) GetOldPrice() string {
	return humanize.FormatFloat("#,###.##", g.oldPrice)
}

func NewXboxStoreGame(title Title, link Link, price PromotionPrice, oldPrice RegularPrice) XboxStoreGame {
	return XboxStoreGame{
		Title:    title,
		link:     link,
		price:    price,
		oldPrice: oldPrice,
	}
}

func (g *XboxStoreGame) IsValidGame() bool {
	_, err := g.GetLink()
	if err != nil {
		return false
	}
	if g.Title == "" {
		return false
	}
	return true
}
