package cmd

import (
	"fmt"

	"github.com/dominikus1993/xbox-promotion-checker-bot/internal/console"
	"github.com/dominikus1993/xbox-promotion-checker-bot/internal/discord"
	"github.com/dominikus1993/xbox-promotion-checker-bot/internal/files"
	"github.com/dominikus1993/xbox-promotion-checker-bot/internal/html"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/filter"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/service"
	broadcast "github.com/dominikus1993/xbox-promotion-checker-bot/pkg/writer"
	log "github.com/sirupsen/logrus"
	"github.com/urfave/cli/v2"
)

const xboxStoreUrl = "https://www.microsoft.com/pl-pl/store/deals/games/xbox"

func XboxGamePromotionParser(context *cli.Context) error {
	webhookId := context.String("webhookid")
	webhooktoken := context.String("webhooktoken")
	log.Infoln("starting xbox game promotion parser")
	fileFilter, err := files.NewTxtFileFilter("./games.txt")
	if err != nil {
		return fmt.Errorf("%w, failed to create file filter", err)
	}
	priceFilter := filter.NewPriceFilter()
	discord, err := discord.NewDiscordXboxGameWriter(webhookId, webhooktoken)
	if err != nil {
		return fmt.Errorf("%w, failed to create discord writer", err)
	}
	consoleW := console.NewConsoleXboxGameWriter()
	broadcaster := broadcast.NewBroadcastXboxGameWriter(discord, consoleW)
	provider := html.NewXboxStoreHtmlParser(xboxStoreUrl)
	return service.NewXboxGamePromotionParser(provider, broadcaster, priceFilter, fileFilter).Parse(context.Context)
}
