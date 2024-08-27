package mongo

import (
	"context"

	"github.com/dominikus1993/go-toolkit/channels"
	data "github.com/dominikus1993/xbox-promotion-checker-bot/pkg/model"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
)

type DatabaseOldPromotionFilter struct {
	client *MongoClient
}

func NewDatabaseOldPromotionFilter(client *MongoClient) *DatabaseOldPromotionFilter {
	return &DatabaseOldPromotionFilter{client: client}
}

func (f *DatabaseOldPromotionFilter) Filter(ctx context.Context, games <-chan data.XboxStoreGame) <-chan data.XboxStoreGame {
	return channels.Filter(games, filterGameThatWasInPromtoionSinceWeek(ctx, f.client), 10)
}

func filterGameThatWasInPromtoionSinceWeek(ctx context.Context, client *MongoClient) func(game data.XboxStoreGame) bool {
	return func(game data.XboxStoreGame) bool {
		col := client.GetCollection()
		opts := options.FindOne()
		res := col.FindOne(ctx, bson.D{{Key: "_id", Value: game.ID}}, opts)
		notexists := res.Err() == mongo.ErrNoDocuments
		return notexists
	}
}
