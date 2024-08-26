package parser

import (
	"context"

	data "github.com/dominikus1993/xbox-promotion-checker-bot/pkg/model"
)

type XboxGameProvider interface {
	Provide(ctx context.Context) <-chan data.XboxStoreGame
}
