package cmd

import (
	"fmt"

	"github.com/dominikus1993/xbox-promotion-checker-bot/internal/console"
	"github.com/dominikus1993/xbox-promotion-checker-bot/internal/files"
	"github.com/dominikus1993/xbox-promotion-checker-bot/internal/html"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/filter"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/service"
	log "github.com/sirupsen/logrus"
	"github.com/urfave/cli/v2"
)

const xboxStoreUrl = "https://www.microsoft.com/pl-pl/store/deals/games/xbox"

func XboxGamePromotionParser(context *cli.Context) error {
	log.Infoln("starting xbox game promotion parser")
	fileFilter, err := files.NewTxtFileFilter("./games.txt")
	if err != nil {
		return fmt.Errorf("%w, failed to create file filter", err)
	}
	priceFilter := filter.NewPriceFilter()
	writer := console.NewConsoleXboxGameWriter()
	provider := html.NewXboxStoreHtmlParser(xboxStoreUrl)
	return service.NewXboxGamePromotionParser(provider, writer, priceFilter, fileFilter).Parse(context.Context)
}
