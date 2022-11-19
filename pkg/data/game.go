package data

import (
	"net/url"

	"github.com/dustin/go-humanize"
)

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

func (g *XboxStoreGame) GetLink() (string, error) {
	uri, err := url.JoinPath("https://www.microsoft.com/", g.link)
	if err != nil {
		return "", err
	}

	return uri, nil
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
