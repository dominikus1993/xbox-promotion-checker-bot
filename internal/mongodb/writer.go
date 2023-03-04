package mongo

import (
	"context"

	"github.com/dominikus1993/go-toolkit/channels"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	log "github.com/sirupsen/logrus"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
	"go.mongodb.org/mongo-driver/x/bsonx"
)

const ttlSeconds = 60 * 60 * 24 * 7 // 7 days

type mongoGameWriter struct {
	client *MongoClient
}

func NewMongoGameWriter(client *MongoClient) *mongoGameWriter {
	return &mongoGameWriter{client: client}
}

func (writer *mongoGameWriter) Write(ctx context.Context, games <-chan data.XboxStoreGame) error {
	collection := writer.client.GetCollection()
	// TTL index
	index := mongo.IndexModel{
		Keys:    bsonx.Doc{{Key: "CrawledAt", Value: bsonx.Int32(1)}},
		Options: options.Index().SetExpireAfterSeconds(ttlSeconds), // Will be removed after 7 days
	}

	_, err := collection.Indexes().CreateOne(context.Background(), index)

	if err != nil {
		return err
	}
	gamesToWrite := channels.ToSlice(toMongoWriteModel(games))
	if len(gamesToWrite) == 0 {
		log.Infoln("no games to store")
		return nil
	}
	_, err = collection.BulkWrite(ctx, gamesToWrite)

	return err
}
