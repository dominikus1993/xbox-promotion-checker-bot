package cmd

import (
	"fmt"

	"github.com/dominikus1993/xbox-promotion-checker-bot/internal/console"
	"github.com/dominikus1993/xbox-promotion-checker-bot/internal/discord"
	"github.com/dominikus1993/xbox-promotion-checker-bot/internal/files"
	"github.com/dominikus1993/xbox-promotion-checker-bot/internal/html"
	mongo "github.com/dominikus1993/xbox-promotion-checker-bot/internal/mongodb"
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
	promotionPercentage := context.Float64("pricePromotionPercentage")
	mongoConnection := context.String("mongo-connection")
	client, err := mongo.NewClient(context.Context, mongoConnection, "Games", "promotions")
	if err != nil {
		return fmt.Errorf("%w, failed to create mongo connection", err)
	}
	defer client.Close(context.Context)
	log.Infoln("starting xbox game promotion parser")
	newPromotionsFilter := mongo.NewDatabaseOldPromotionFilter(client)
	mongoPromotionsWriter := mongo.NewMongoGameWriter(client)

	fileFilter, err := files.NewTxtFileFilter(files.NewFileGameThatIWantProvider("./games.txt"))
	if err != nil {
		return fmt.Errorf("%w, failed to create file filter", err)
	}
	priceFilter := filter.NewPriceFilter(promotionPercentage)
	discord, err := discord.NewDiscordXboxGameWriter(webhookId, webhooktoken)
	if err != nil {
		return fmt.Errorf("%w, failed to create discord writer", err)
	}
	uniqueFilter := filter.NewUniqeFilter()
	consoleW := console.NewConsoleXboxGameWriter()
	broadcaster := broadcast.NewBroadcastXboxGameWriter(discord, consoleW, mongoPromotionsWriter)
	provider := html.NewXboxStoreHtmlParser(xboxStoreUrl, html.NewCollyCollector())
	return service.NewXboxGamePromotionParser(provider, broadcaster, uniqueFilter, priceFilter, fileFilter, newPromotionsFilter).Parse(context.Context)
}
