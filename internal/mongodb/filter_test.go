package mongo

import (
	"context"
	"fmt"
	"testing"

	"github.com/dominikus1993/go-toolkit/channels"
	"github.com/dominikus1993/go-toolkit/random"
	"github.com/dominikus1993/integrationtestcontainers-go/mongodb"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"github.com/stretchr/testify/assert"
)

func TestFilter(t *testing.T) {
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

	filter := NewDatabaseOldPromotionFilter(client)

	t.Run("Filter game that exists", func(t *testing.T) {
		// Act
		game := data.NewXboxStoreGame(random.String(10), "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 21.37, 69.0)

		err := writer.Write(ctx, channels.FromSlice([]data.XboxStoreGame{game}))
		assert.NoError(t, err)

		resultStream := filter.Filter(ctx, channels.FromSlice([]data.XboxStoreGame{game}))

		subject := channels.ToSlice(resultStream)

		assert.Empty(t, subject)
	})

	t.Run("Filter game that not exists", func(t *testing.T) {
		// Act
		game := data.NewXboxStoreGame(random.String(10), "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 21.37, 69.0)

		err := writer.Write(ctx, channels.FromSlice([]data.XboxStoreGame{game}))
		assert.NoError(t, err)

		resultStream := filter.Filter(ctx, channels.FromSlice([]data.XboxStoreGame{data.NewXboxStoreGame(random.String(10), "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 21.37, 69.0)}))

		subject := channels.ToSlice(resultStream)

		assert.Len(t, subject, 1)
	})

	t.Run("Filter games", func(t *testing.T) {
		// Act
		game := data.NewXboxStoreGame(random.String(10), "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 21.37, 69.0)
		game2 := data.NewXboxStoreGame(random.String(10), "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 21.37, 69.0)
		games := []data.XboxStoreGame{game, game2}
		err := writer.Write(ctx, channels.FromSlice([]data.XboxStoreGame{game}))
		assert.NoError(t, err)

		resultStream := filter.Filter(ctx, channels.FromSlice(games))

		subject := channels.ToSlice(resultStream)

		assert.Len(t, subject, 1)
		assert.Equal(t, game2, subject[0])
	})
}
