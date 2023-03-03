package mongo

import (
	"context"

	log "github.com/sirupsen/logrus"
	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
)

type MongoClient struct {
	*mongo.Client
	db *mongo.Database
}

func NewClient(ctx context.Context, connectionString, database string) (*MongoClient, error) {

	// Set client options
	clientOptions := options.Client().ApplyURI(connectionString)

	// connect to MongoDB
	client, err := mongo.Connect(ctx, clientOptions)

	if err != nil {
		return nil, err
	}

	// Check the connection
	err = client.Ping(ctx, nil)

	if err != nil {
		return nil, err
	}
	db := client.Database(database)

	return &MongoClient{Client: client, db: db}, nil
}

func (c *MongoClient) GetCollection(name string) *mongo.Collection {
	return c.db.Collection(name)
}

func (c *MongoClient) Close(ctx context.Context) {
	if err := c.Disconnect(ctx); err != nil {
		log.WithError(err).Error("Error when trying disconnect from mongo")
	}
}
