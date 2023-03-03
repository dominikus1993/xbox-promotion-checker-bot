package mongo

import (
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"go.mongodb.org/mongo-driver/bson/primitive"
)

type screapedXboxHame struct {
	ID        string             `bson:"_id,omitempty"`
	Title     string             `bson:"Title"`
	Link      string             `bson:"Link"`
	Price     float64            `bson:"Price"`
	OldPrice  float64            `bson:"OldPrice"`
	CrawledAt primitive.DateTime `bson:"CrawledAt"`
}

func fromXboxGame(game data.XboxStoreGame) *screapedXboxHame {

}
