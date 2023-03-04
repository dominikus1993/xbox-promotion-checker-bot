package mongo

import (
	"time"

	"github.com/dominikus1993/go-toolkit/crypto"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
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
	id, err := crypto.GenerateId(game.Title)
	if err != nil {
		panic(err)
	}
	link, err := game.GetLink()

	if err != nil {
		panic(err)
	}

	return screapedXboxHame{
		ID:        id,
		Title:     game.Title,
		Link:      link.String(),
		Price:     game.FormatPrice(),
		Promotion: game.FormatPromotionPercentage(),
		OldPrice:  game.FormatOldPrice(),
		CrawledAt: primitive.NewDateTimeFromTime(time.Now()),
	}
}

func toMongoWriteModel(games <-chan data.XboxStoreGame) []mongo.WriteModel {
	result := make([]mongo.WriteModel, 0)
	for game := range games {
		mongoGame := mongo.NewInsertOneModel().SetDocument(fromXboxGame(game))
		result = append(result, mongoGame)
	}
	return result
}
