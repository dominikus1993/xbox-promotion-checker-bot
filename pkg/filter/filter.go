package filter

import "github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"

type GameFilter interface {
	Filter(games <-chan data.XboxStoreGame) <-chan data.XboxStoreGame
}

func FilterPipeline(stream <-chan data.XboxStoreGame, predicates ...GameFilter) <-chan data.XboxStoreGame {
	out := stream
	for _, predicate := range predicates {
		tmp := out
		out = predicate.Filter(tmp)
	}
	return out
}
