package data

type XboxStoreGame struct {
	Title    string
	Link     string
	Price    *float64
	OldPrice *float64
}

func NewXboxStoreGame(title, link string, price, oldPrice *float64) XboxStoreGame {
	return XboxStoreGame{
		Title:    title,
		Link:     link,
		Price:    price,
		OldPrice: oldPrice,
	}
}
