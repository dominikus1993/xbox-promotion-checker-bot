package filter

import (
	"context"

	"github.com/dominikus1993/go-toolkit/channels"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
)

type PriceFilter struct {
	promotionPercentage float64
}

func NewPriceFilter(promotionPercentage float64) *PriceFilter {
	return &PriceFilter{promotionPercentage: promotionPercentage}
}

func (f *PriceFilter) Filter(ctx context.Context, games <-chan data.XboxStoreGame) <-chan data.XboxStoreGame {
	return channels.Filter(games, func(game data.XboxStoreGame) bool {
		return game.CalculatePromotionPercentage() >= f.promotionPercentage
	}, 10)
}
