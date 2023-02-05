package files

import (
	"bufio"
	"fmt"
	"os"
	"strings"
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
