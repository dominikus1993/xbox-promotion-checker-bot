package parser

import (
	"context"

	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
)

type XboxGameParser interface {
	Parse(ctx context.Context) <-chan data.XboxStoreGame
}
