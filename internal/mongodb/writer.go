package mongo

import (
	"context"
	"errors"

	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"go.mongodb.org/mongo-driver/bson"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
)

var errEmptyArray = errors.New("games array is empty")

const ttlSeconds = 60 * 60 * 24 * 7 // 7 days

type mongoGameWriter struct {
	client *MongoClient
}

func NewMongoGameWriter(client *MongoClient) *mongoGameWriter {
	return &mongoGameWriter{client: client}
}

func (writer *mongoGameWriter) Write(ctx context.Context, games []data.XboxStoreGame) error {
	if len(games) == 0 {
		return errEmptyArray
	}

	collection := writer.client.GetCollection()
	// TTL index
	index := mongo.IndexModel{
		Keys:    bson.D{{Key: "CrawledAt", Value: 1}},
		Options: options.Index().SetExpireAfterSeconds(ttlSeconds), // Will be removed after 7 days
	}

	_, err := collection.Indexes().CreateOne(ctx, index)

	if err != nil {
		return err
	}
	gamesToWrite := toMongoWriteModel(games)
	_, err = collection.BulkWrite(ctx, gamesToWrite)

	return err
}
