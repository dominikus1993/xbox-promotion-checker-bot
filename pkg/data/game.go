package data

type XboxStoreGame struct {
	Title    string
	Link     string
	Image    string
	Price    *float64
	OldPrice *float64
}

func NewXboxStoreGame(title, link, image string, price, oldPrice *float64) XboxStoreGame {
	return XboxStoreGame{
		Title:    title,
		Link:     link,
		Image:    image,
		Price:    price,
		OldPrice: oldPrice,
	}
}
