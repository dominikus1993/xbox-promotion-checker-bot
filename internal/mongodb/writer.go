package mongo

import (
	"context"
	"errors"

	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
	"go.mongodb.org/mongo-driver/x/bsonx"
)

var errEmptyArray = errors.New("games array is empty")

const sevenDays = 60 * 60 * 24 * 7

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
		Keys:    bsonx.Doc{{Key: "CrawledAt", Value: bsonx.Int32(1)}},
		Options: options.Index().SetExpireAfterSeconds(sevenDays),
	}

	_, err := collection.Indexes().CreateOne(ctx, index)

	if err != nil {
		return err
	}
	gamesToWrite := toMongoWriteModel(games)
	_, err = collection.BulkWrite(ctx, gamesToWrite)

	return err
}
