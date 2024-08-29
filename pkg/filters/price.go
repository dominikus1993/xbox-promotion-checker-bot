package filters

import (
	"context"

	"github.com/dominikus1993/go-toolkit/channels"
	data "github.com/dominikus1993/xbox-promotion-checker-bot/pkg/model"
)

type PriceFilter struct {
	promotionPercentage float64
}

func NewPriceFilter(promotionPercentage float64) *PriceFilter {
	return &PriceFilter{promotionPercentage: promotionPercentage}
}

func (f *PriceFilter) Filter(ctx context.Context, games <-chan data.XboxStoreGame) <-chan data.XboxStoreGame {
	return channels.Filter(games, func(game data.XboxStoreGame) bool {
		return pricePredicate(game, f.promotionPercentage)
	}, 10)
}

func pricePredicate(game data.XboxStoreGame, promotionPercentage float64) bool {
	return game.CalculatePromotionPercentage() >= promotionPercentage
}
