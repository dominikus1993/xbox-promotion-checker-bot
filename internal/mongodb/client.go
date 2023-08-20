package mongo

import (
	"context"

	"go.mongodb.org/mongo-driver/mongo"
	"go.mongodb.org/mongo-driver/mongo/options"
	"golang.org/x/exp/slog"
)

type MongoClient struct {
	*mongo.Client
	db         *mongo.Database
	collection string
}

func NewClient(ctx context.Context, connectionString, database, collection string) (*MongoClient, error) {

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

	return &MongoClient{Client: client, db: db, collection: collection}, nil
}

func (c *MongoClient) GetCollection() *mongo.Collection {
	return c.db.Collection(c.collection)
}

func (c *MongoClient) Close(ctx context.Context) {
	if err := c.Disconnect(ctx); err != nil {
		slog.Error("Error when trying disconnect from mongo", "error", err)
	}
}
