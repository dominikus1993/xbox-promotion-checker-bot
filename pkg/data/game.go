package data

import (
	"net/url"

	"github.com/dustin/go-humanize"
)

type XboxStoreGame struct {
	Title    string
	link     string
	price    float64
	oldPrice float64
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

func (g *XboxStoreGame) GetPrice() string {
	return humanize.FormatFloat("#,###.##", g.price)
}

func (g *XboxStoreGame) GetOldPrice() string {
	return humanize.FormatFloat("#,###.##", g.oldPrice)
}

func NewXboxStoreGame(title, link string, price, oldPrice float64) XboxStoreGame {
	return XboxStoreGame{
		Title:    title,
		link:     link,
		price:    price,
		oldPrice: oldPrice,
	}
}
