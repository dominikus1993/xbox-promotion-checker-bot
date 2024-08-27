package mongo

import (
	"time"

	data "github.com/dominikus1993/xbox-promotion-checker-bot/pkg/model"
	"github.com/samber/lo"
	"go.mongodb.org/mongo-driver/bson/primitive"
	"go.mongodb.org/mongo-driver/mongo"
)

type screapedXboxHame struct {
	ID        string             `bson:"_id,omitempty"`
	Title     string             `bson:"Title"`
	Link      string             `bson:"Link"`
	Price     string             `bson:"Price"`
	OldPrice  string             `bson:"OldPrice"`
	Promotion string             `bson:"Promotion"`
	CrawledAt primitive.DateTime `bson:"CrawledAt"`
}

func fromXboxGame(game data.XboxStoreGame) screapedXboxHame {
	link, err := game.GetLink()

	if err != nil {
		panic(err)
	}

	return screapedXboxHame{
		ID:        game.ID,
		Title:     game.Title,
		Link:      link.String(),
		Price:     game.FormatPrice(),
		Promotion: game.FormatPromotionPercentage(),
		OldPrice:  game.FormatOldPrice(),
		CrawledAt: primitive.NewDateTimeFromTime(time.Now()),
	}
}

func toMongoWriteModel(games []data.XboxStoreGame) []mongo.WriteModel {
	return lo.Map(games, func(game data.XboxStoreGame, _ int) mongo.WriteModel {
		return mongo.NewInsertOneModel().SetDocument(fromXboxGame(game))
	})
}
