package mongo

import (
	"context"
	"fmt"
	"testing"

	channels "github.com/dominikus1993/go-toolkit/channels"
	"github.com/dominikus1993/go-toolkit/random"
	"github.com/dominikus1993/integrationtestcontainers-go/mongodb"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"github.com/stretchr/testify/assert"
)

func TestWritere(t *testing.T) {
	if testing.Short() {
		t.Skip("skipping integration test")
	}
	collectionName := "promotions"
	// Arrange
	ctx := context.Background()
	mongoC, err := mongodb.StartContainer(ctx, mongodb.NewMongoContainerConfigurationBuilder().Build())
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

	writer := NewMongoGameWriter(client)

	t.Run("Write game", func(t *testing.T) {
		// Act
		game := data.NewXboxStoreGame(random.String(10), "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 21.37, 69.0)

		err := writer.Write(ctx, channels.FromSlice([]data.XboxStoreGame{game}))
		assert.NoError(t, err)
	})

	t.Run("Write twice game", func(t *testing.T) {
		// Act
		game := data.NewXboxStoreGame(random.String(10), "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 21.37, 69.0)
		game2 := data.NewXboxStoreGame(random.String(10), "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 21.37, 69.0)
		err := writer.Write(ctx, channels.FromSlice([]data.XboxStoreGame{game}))
		assert.NoError(t, err)

		err = writer.Write(ctx, channels.FromSlice([]data.XboxStoreGame{game2}))
		assert.NoError(t, err)
	})
}
