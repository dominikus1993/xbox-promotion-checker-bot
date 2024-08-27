package discord

import (
	"testing"

	data "github.com/dominikus1993/xbox-promotion-checker-bot/pkg/model"
	"github.com/stretchr/testify/assert"
)

func TestCreateEmbeds(t *testing.T) {
	data := []data.XboxStoreGame{
		data.NewXboxStoreGame("", "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 10.0, 10.0),
		data.NewXboxStoreGame("", "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 10.0, 10.0),
		data.NewXboxStoreGame("", "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 10.0, 20.0),
		data.NewXboxStoreGame("", "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 10.0, 100.0),
		data.NewXboxStoreGame("", "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 134.99, 269.99),
	}

	embeds, err := createEmbeds(data)
	assert.NoError(t, err)

	assert.Len(t, embeds, len(data))
}

// BenchmarkCreateEmbeds-8            79268             13385 ns/op            7955 B/op        147 allocs/op
func BenchmarkCreateEmbeds(b *testing.B) {
	data := []data.XboxStoreGame{
		data.NewXboxStoreGame("", "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 10.0, 10.0),
		data.NewXboxStoreGame("", "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 10.0, 10.0),
		data.NewXboxStoreGame("", "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 10.0, 20.0),
		data.NewXboxStoreGame("", "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 10.0, 100.0),
		data.NewXboxStoreGame("", "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 134.99, 269.99),
	}

	for i := 0; i < b.N; i++ {
		_, _ = createEmbeds(data)
	}
}
