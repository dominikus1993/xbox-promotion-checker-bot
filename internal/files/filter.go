package files

import (
	"bufio"
	"fmt"
	"os"
	"strings"

	"github.com/dominikus1993/xbox-promotion-checker-bot/internal/channels"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
)

type TxtFileFilter struct {
	gamesThatIWantBuy []string
}

func NewTxtFileFilter(filePath string) (*TxtFileFilter, error) {
	f, err := os.Open(filePath)

	if err != nil {
		return nil, fmt.Errorf("%w, failed to open file", err)
	}

	defer f.Close()

	scanner := bufio.NewScanner(f)
	gamesThatIWantBuy := make([]string, 0)
	for scanner.Scan() {
		gamesThatIWantBuy = append(gamesThatIWantBuy, strings.ToLower(scanner.Text()))
	}

	if err := scanner.Err(); err != nil {
		return nil, fmt.Errorf("%w, failed to read file", err)
	}

	return &TxtFileFilter{gamesThatIWantBuy: gamesThatIWantBuy}, nil
}

func normalizedTitle(game data.XboxStoreGame) string {
	return strings.ToLower(game.Title)
}

func (f *TxtFileFilter) Filter(games <-chan data.XboxStoreGame) <-chan data.XboxStoreGame {
	return channels.Filter(games, func(game data.XboxStoreGame) bool {
		normalizedTitle := normalizedTitle(game)
		exists := false
		for _, g := range f.gamesThatIWantBuy {
			if strings.Contains(normalizedTitle, g) {
				exists = true
				break
			}
		}
		return exists
	})
}
