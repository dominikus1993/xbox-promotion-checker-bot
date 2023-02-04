package files

import (
	"bufio"
	"fmt"
	"os"
	"strings"

	"github.com/dominikus1993/xbox-promotion-checker-bot/internal/channels"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
)

type GameThatIWantProvider interface {
	Provide() ([]string, error)
}

type fileGameThatIWantProvider struct {
	filePath string
}

func NewFileGameThatIWantProvider(filePath string) *fileGameThatIWantProvider {
	return &fileGameThatIWantProvider{filePath: filePath}
}

func (p *fileGameThatIWantProvider) Provide() ([]string, error) {
	f, err := os.Open(p.filePath)

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

	return gamesThatIWantBuy, nil
}

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
	return channels.Filter(games, filterGameInFile(f.gamesThatIWantBuy))
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
