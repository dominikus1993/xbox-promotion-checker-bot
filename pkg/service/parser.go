package service

import (
	"context"
	"log/slog"

	"github.com/dominikus1993/go-toolkit/channels"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/filter"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/parser"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/writer"
)

type XboxGamePromotionParser struct {
	filters  []filter.GameFilter
	provider parser.XboxGameProvider
	writer   writer.XboxGameWriter
}

func NewXboxGamePromotionParser(provider parser.XboxGameProvider, writer writer.XboxGameWriter, filters ...filter.GameFilter) *XboxGamePromotionParser {
	return &XboxGamePromotionParser{
		filters:  filters,
		provider: provider,
		writer:   writer,
	}
}

func (svc *XboxGamePromotionParser) Parse(ctx context.Context) error {
	games := svc.provider.Provide(ctx)
	result := filter.FilterPipeline(ctx, games, svc.filters...)

	gamesresult := channels.ToSlice(result)
	if len(gamesresult) == 0 {
		slog.Info("no games in promotion")
		return nil
	}

	return svc.writer.Write(ctx, gamesresult)
}
