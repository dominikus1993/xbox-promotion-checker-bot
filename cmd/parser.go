package cmd

import (
	"errors"

	"github.com/urfave/cli/v2"
)

const xboxStoreUrl = "https://www.microsoft.com/pl-pl/store/deals/games/xbox"

func XboxGamePromotionParser(context *cli.Context) error {
	// webhookId := context.String("webhookid")
	// webhooktoken := context.String("webhooktoken")
	// log.Infoln("starting xbox game promotion parser")
	// fileFilter, err := files.NewTxtFileFilter("./games.txt")
	// if err != nil {
	// 	return fmt.Errorf("%w, failed to create file filter", err)
	// }
	// priceFilter := filter.NewPriceFilter()
	// discord, err := discord.NewDiscordXboxGameWriter(webhookId, webhooktoken)
	// if err != nil {
	// 	return fmt.Errorf("%w, failed to create discord writer", err)
	// }
	// consoleW := console.NewConsoleXboxGameWriter()
	// broadcaster := broadcast.NewBroadcastXboxGameWriter(discord, consoleW)
	// provider := html.NewXboxStoreHtmlParser(xboxStoreUrl, html.NewCollyCollector())
	// return service.NewXboxGamePromotionParser(provider, broadcaster, priceFilter, fileFilter).Parse(context.Context)

	return errors.New("not Implemented")
}
