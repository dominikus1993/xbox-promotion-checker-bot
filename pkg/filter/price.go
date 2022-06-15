package filter

import (
	"github.com/dominikus1993/xbox-promotion-checker-bot/internal/channels"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
)

type PriceFilter struct {
}

func NewPriceFilter() *PriceFilter {
	return &PriceFilter{}
}

func calculatePromotionPercentage(oldPrice, price *float64) float64 {
	if oldPrice == nil || price == nil {
		return 0
	}
	return 100 - (*price / *oldPrice * 100)
}

func (f *PriceFilter) Filter(games <-chan data.XboxStoreGame) <-chan data.XboxStoreGame {
	return channels.Filter(games, func(game data.XboxStoreGame) bool {
		if game.Price != nil && game.OldPrice != nil {
			return calculatePromotionPercentage(game.OldPrice, game.Price) >= 75
		}
		return false
	})
}
