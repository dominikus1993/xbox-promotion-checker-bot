package files

import (
	"context"
	"strings"

	"github.com/dominikus1993/go-toolkit/channels"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"github.com/lithammer/fuzzysearch/fuzzy"
)

type TxtFileFilter struct {
	gamesThatIWantBuy []string
}

func NewTxtFileFilter(provider GameThatIWantProvider) (*TxtFileFilter, error) {
	gamesThatIWantBuy, err := provider.Provide()
	if err != nil {
		return nil, err
	}
	return &TxtFileFilter{gamesThatIWantBuy: gamesThatIWantBuy}, nil
}

func normalizedTitle(game data.XboxStoreGame) string {
	trimmedTitle := strings.TrimSpace(game.Title)
	return strings.ToLower(trimmedTitle)
}

func (f *TxtFileFilter) Filter(ctx context.Context, games <-chan data.XboxStoreGame) <-chan data.XboxStoreGame {
	return channels.Filter(games, filterGameInFile(f.gamesThatIWantBuy), 10)
}

func filterGameInFile(gamesThatIWantBuy []string) func(game data.XboxStoreGame) bool {
	return func(game data.XboxStoreGame) bool {
		normalizedTitle := normalizedTitle(game)
		for _, g := range gamesThatIWantBuy {
			if fuzzy.MatchNormalizedFold(g, normalizedTitle) {
				return true
			}
		}
		return false
	}
}
