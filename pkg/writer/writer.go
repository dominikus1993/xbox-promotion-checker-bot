package writer

import (
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"golang.org/x/sync/errgroup"
)

type XboxGameWriter interface {
	Write(games <-chan data.XboxStoreGame) error
}

type BroadcastXboxGameWriter struct {
	writers []XboxGameWriter
}

func NewBroadcastXboxGameWriter(writers ...XboxGameWriter) *BroadcastXboxGameWriter {
	return &BroadcastXboxGameWriter{writers: writers}
}

func (writer *BroadcastXboxGameWriter) Write(games <-chan data.XboxStoreGame) error {
	var wg errgroup.Group
	streams := fanOut(games, len(writer.writers))
	for i, notifier := range writer.writers {
		not := notifier
		stream := streams[i]
		wg.Go(func() error {
			return not.Write(stream)
		})
	}
	return wg.Wait()
}

func fanOut[T any](stream <-chan T, len int) []chan T {
	out := make([]chan T, len)
	for i := 0; i < len; i++ {
		out[i] = make(chan T)
	}

	go func() {
		defer closeAll(out)
		for record := range stream {
			writeToAll(record, out)
		}
	}()
	return out
}

func writeToAll[T any](elem T, streams []chan T) {
	for _, channel := range streams {
		channel <- elem
	}
}

func closeAll[T any](streams []chan T) {
	for _, channel := range streams {
		close(channel)
	}
}
