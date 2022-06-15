package parser

import (
	"context"

	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
)

type XboxGameProvider interface {
	Provide(ctx context.Context) <-chan data.XboxStoreGame
}
