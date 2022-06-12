package filter

import "github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"

type GameFilter interface {
	Filter(games <-chan data.XboxStoreGame) <-chan data.XboxStoreGame
}
