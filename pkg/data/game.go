package data

import "net/url"

type XboxStoreGame struct {
	Title    string
	link     string
	Price    *float64
	OldPrice *float64
}

func (g *XboxStoreGame) GetLink() (string, error) {
	uri, err := url.JoinPath("https://www.xbox.com/", g.link)
	if err != nil {
		return "", err
	}

	return uri, nil
}

func NewXboxStoreGame(title, link string, price, oldPrice *float64) XboxStoreGame {
	return XboxStoreGame{
		Title:    title,
		link:     link,
		Price:    price,
		OldPrice: oldPrice,
	}
}
