package filter

import (
	"context"

	data "github.com/dominikus1993/xbox-promotion-checker-bot/pkg/model"
)

type GameFilter interface {
	Filter(ctx context.Context, games <-chan data.XboxStoreGame) <-chan data.XboxStoreGame
}

func FilterPipeline(ctx context.Context, stream <-chan data.XboxStoreGame, predicates ...GameFilter) <-chan data.XboxStoreGame {
	out := stream
	for _, predicate := range predicates {
		tmp := out
		out = predicate.Filter(ctx, tmp)
	}
	return out
}
