package files

import (
	"context"
	"testing"

	gotolkit "github.com/dominikus1993/go-toolkit/channels"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"github.com/stretchr/testify/assert"
)

func mapTitles(games []data.XboxStoreGame) []string {
	var titles []string
	for _, game := range games {
		titles = append(titles, game.Title)
	}
	return titles
}

func TestParsingFirstPage(t *testing.T) {
	filter := TxtFileFilter{gamesThatIWantBuy: []string{"stellaris", "cyberpunk"}}
	games := gotolkit.FromSlice([]data.XboxStoreGame{{Title: "Cyberpunk 2077"}, {Title: "Stellaris Enchanced"}, {Title: "Assasins Creed"}})
	result := filter.Filter(context.TODO(), games)
	got := gotolkit.ToSlice(result)

	assert.NotNil(t, got)
	assert.NotEmpty(t, got)
	titlesThatWant := []string{"Cyberpunk 2077", "Stellaris Enchanced"}

	assert.ElementsMatch(t, titlesThatWant, mapTitles(got))
}

// BenchmarkParsingFirstPage-8      2056284               588.8 ns/op           816 B/op          4 allocs/op
func BenchmarkParsingFirstPage(b *testing.B) {
	filter := TxtFileFilter{gamesThatIWantBuy: []string{"stellaris", "cyberpunk"}}
	games := gotolkit.FromSlice([]data.XboxStoreGame{{Title: "Cyberpunk 2077"}, {Title: "Stellaris Enchanced"}, {Title: "Assasins Creed"}})

	for i := 0; i < b.N; i++ {
		result := filter.Filter(context.TODO(), games)
		gotolkit.ToSlice(result)
	}
}
