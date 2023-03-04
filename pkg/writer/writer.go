package writer

import (
	"context"

	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"github.com/samber/lo"
	"golang.org/x/sync/errgroup"
)

type XboxGameWriter interface {
	Write(ctx context.Context, games <-chan data.XboxStoreGame) error
}

type BroadcastXboxGameWriter struct {
	writers []XboxGameWriter
}

func NewBroadcastXboxGameWriter(writers ...XboxGameWriter) *BroadcastXboxGameWriter {
	return &BroadcastXboxGameWriter{writers: writers}
}

func (writer *BroadcastXboxGameWriter) Write(ctx context.Context, games <-chan data.XboxStoreGame) error {
	var wg errgroup.Group
	streams := lo.FanOut(len(writer.writers), 10, games)
	for i, notifier := range writer.writers {
		not := notifier
		stream := streams[i]
		wg.Go(func() error {
			return not.Write(ctx, stream)
		})
	}
	return wg.Wait()
}
