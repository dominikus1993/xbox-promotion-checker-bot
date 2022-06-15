package main

import (
	"os"

	"github.com/dominikus1993/xbox-promotion-checker-bot/cmd"
	log "github.com/sirupsen/logrus"
	"github.com/urfave/cli/v2"
)

func main() {
	app := &cli.App{
		Name:   "xbox-promotion-bot",
		Usage:  "parse xbox game promotions",
		Action: cmd.XboxGamePromotionParser,
	}
	err := app.Run(os.Args)
	if err != nil {
		log.WithError(err).Fatalln("error running app")
	}
}
