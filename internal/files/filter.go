package files

import (
	"strings"

	"github.com/dominikus1993/xbox-promotion-checker-bot/internal/channels"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
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
	return strings.ToLower(game.Title)
}

func (f *TxtFileFilter) Filter(games <-chan data.XboxStoreGame) <-chan data.XboxStoreGame {
	return channels.Filter(games, filterGameInFile(f.gamesThatIWantBuy), 10)
}

func filterGameInFile(gamesThatIWantBuy []string) func(game data.XboxStoreGame) bool {
	return func(game data.XboxStoreGame) bool {
		normalizedTitle := normalizedTitle(game)
		exists := false
		for _, g := range gamesThatIWantBuy {
			if strings.Contains(normalizedTitle, g) {
				exists = true
				break
			}
		}
		return exists
	}
}
