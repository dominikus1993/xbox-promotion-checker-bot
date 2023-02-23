package files

import (
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
	result := filter.Filter(games)
	got := gotolkit.ToSlice(result)

	assert.NotNil(t, got)
	assert.NotEmpty(t, got)
	titlesThatWant := []string{"Cyberpunk 2077", "Stellaris Enchanced"}

	assert.ElementsMatch(t, titlesThatWant, mapTitles(got))
}
