package console

import (
	"context"

	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"github.com/k0kubun/pp/v3"
)

type ConsoleXboxGameWriter struct {
}

func NewConsoleXboxGameWriter() *ConsoleXboxGameWriter {
	return &ConsoleXboxGameWriter{}
}

func printGame(game data.XboxStoreGame) {
	pp.Println(game)
}

func (w *ConsoleXboxGameWriter) Write(ctx context.Context, games []data.XboxStoreGame) error {
	scheme := pp.ColorScheme{
		Integer: pp.Green | pp.Bold,
		Float:   pp.Black | pp.BackgroundWhite | pp.Bold,
		String:  pp.Yellow,
	}

	// Register it for usage
	pp.SetColorScheme(scheme)
	for _, game := range games {
		printGame(game)
	}
	return nil
}
