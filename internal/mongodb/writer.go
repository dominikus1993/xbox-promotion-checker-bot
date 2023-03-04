package mongo

import (
	"context"
	"time"

	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
	"go.mongodb.org/mongo-driver/x/bsonx"
)

type MongoGameWriter struct {
	client *MongoClient
}

func (writer *MongoGameWriter) Write(ctx context.Context, games <-chan data.XboxStoreGame) error {
	collection := writer.client.GetCollection()
	// TTL index
	index := mongo.IndexModel{
		Keys:    bsonx.Doc{{Key: "CrawledAt", Value: bsonx.Int32(1)}},
		Options: options.Index().SetExpireAfterSeconds(int32(time.Now().Add(time.Hour * 24).Unix())), // Will be removed after 24 Hours.
	}

	_, err := collection.Indexes().CreateOne(context.Background(), index)

	if err != nil {
		return err
	}

	dbGames := fromStream(games)

	_, err = collection.BulkWrite(ctx, dbGames)

	if err != nil {
		return err
	}
}
