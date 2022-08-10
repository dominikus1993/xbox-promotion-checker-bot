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

func (f *PriceFilter) Filter(games <-chan data.XboxStoreGame) <-chan data.XboxStoreGame {
	return channels.Filter(games, func(game data.XboxStoreGame) bool {
		return game.CalculatePromotionPercentage() >= 50
	})
}
