package filters

import (
	"context"

	"github.com/dominikus1993/go-toolkit/channels"
	data "github.com/dominikus1993/xbox-promotion-checker-bot/pkg/model"
)

type uniqueFilter struct {
}

func NewUniqeFilter() *uniqueFilter {
	return &uniqueFilter{}
}

func (f *uniqueFilter) Filter(ctx context.Context, games <-chan data.XboxStoreGame) <-chan data.XboxStoreGame {
	return channels.UniqBy(games, func(game data.XboxStoreGame) data.GameID { return game.ID }, 10)
}
