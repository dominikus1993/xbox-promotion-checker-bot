package mongo

import (
	"testing"

	"github.com/dominikus1993/go-toolkit/channels"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"github.com/stretchr/testify/assert"
)

func TestFormatPercentage(t *testing.T) {
	data := []data.XboxStoreGame{
		data.NewXboxStoreGame("", "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 10.0, 10.0),
		data.NewXboxStoreGame("", "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 10.0, 10.0),
		data.NewXboxStoreGame("", "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 10.0, 20.0),
		data.NewXboxStoreGame("", "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 10.0, 100.0),
		data.NewXboxStoreGame("", "https://www.xbox.com/pl-pl/games/store/lego-batman-3-beyond-gotham-deluxe-edition/c4hfhz44z3r3", 134.99, 269.99),
	}
	stream := channels.FromSlice(data)

	subject := toMongoWriteModel(stream)

	assert.Len(t, subject, len(data))
}
