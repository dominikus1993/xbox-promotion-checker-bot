package writer

import (
	"context"

	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"golang.org/x/sync/errgroup"
)

type XboxGameWriter interface {
	Write(ctx context.Context, games []data.XboxStoreGame) error
}

type BroadcastXboxGameWriter struct {
	writers []XboxGameWriter
}

func NewBroadcastXboxGameWriter(writers ...XboxGameWriter) *BroadcastXboxGameWriter {
	return &BroadcastXboxGameWriter{writers: writers}
}

func (writer *BroadcastXboxGameWriter) Write(ctx context.Context, games []data.XboxStoreGame) error {
	var wg errgroup.Group
	for _, notifier := range writer.writers {
		not := notifier
		wg.Go(func() error {
			return not.Write(ctx, games)
		})
	}
	return wg.Wait()
}
