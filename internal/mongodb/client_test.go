package mongo

import (
	"context"
	"fmt"
	"testing"

	"github.com/stretchr/testify/assert"
	"github.com/testcontainers/testcontainers-go/modules/mongodb"
	"go.mongodb.org/mongo-driver/mongo/readpref"
)

func TestGetCollection(t *testing.T) {
	if testing.Short() {
		t.Skip("skipping integration test")
	}
	collectionName := "promotions"
	// Arrange
	ctx := context.Background()
	mongoC, err := mongodb.Run(ctx, "mongo:6")
	if err != nil {
		t.Fatal(err)
	}

	t.Cleanup(func() {
		if err := mongoC.Terminate(ctx); err != nil {
			t.Fatalf("failed to terminate container: %s", err)
		}
	})

	connectionString, err := mongoC.ConnectionString(ctx)
	if err != nil {
		t.Fatal(fmt.Errorf("can't download mongo conectionstring, %w", err))
	}
	client, err := NewClient(ctx, connectionString, "Games", collectionName)
	if err != nil {
		t.Fatal(err)
	}

	t.Cleanup(func() {
		client.Close(ctx)
	})

	t.Run("Ping", func(t *testing.T) {
		err := client.Ping(ctx, readpref.PrimaryPreferred())

		assert.NoError(t, err)
	})

	t.Run("Get Collection", func(t *testing.T) {
		collection := client.GetCollection()
		assert.NotNil(t, collection)
		assert.Equal(t, collectionName, collection.Name())
	})
}
