package parser

import (
	"context"
	"log/slog"

	"github.com/dominikus1993/go-toolkit/channels"
	filter "github.com/dominikus1993/xbox-promotion-checker-bot/pkg/filters"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/writer"
)

type XboxGamePromotionParser struct {
	filters  []filter.GameFilter
	provider XboxGameProvider
	writer   writer.XboxGameWriter
}

func NewXboxGamePromotionParser(provider XboxGameProvider, writer writer.XboxGameWriter, filters ...filter.GameFilter) *XboxGamePromotionParser {
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
