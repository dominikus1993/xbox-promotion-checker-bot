package service

import (
	"context"

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
	result := filter.FilterPipeline(games, svc.filters...)
	return svc.writer.Write(result)
}
