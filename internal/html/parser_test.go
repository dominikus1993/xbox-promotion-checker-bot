package html

import (
	"context"
	"testing"

	gotolkit "github.com/dominikus1993/go-toolkit"
	"github.com/stretchr/testify/assert"
)

func TestParsingFirstPage(t *testing.T) {
	parser := XboxStoreHtmlParser{xboxStoreUrl: "https://www.microsoft.com/pl-pl/store/deals/games/xbox"}
	result := parser.parsePage(context.Background(), 1)
	subject := gotolkit.ToSlice(result)
	assert.NotNil(t, subject)
	assert.NotEmpty(t, subject)
	for _, game := range subject {
		assert.NotEmpty(t, game.Title)
		assert.NotEmpty(t, game.Link)
		assert.NotEmpty(t, game.OldPrice)
		assert.NotEmpty(t, game.Price)
	}
}

func TestParsingAllPages(t *testing.T) {
	parser := XboxStoreHtmlParser{xboxStoreUrl: "https://www.microsoft.com/pl-pl/store/deals/games/xbox"}
	result := parser.Provide(context.Background())
	subject := gotolkit.ToSlice(result)
	assert.NotNil(t, subject)
	assert.NotEmpty(t, subject)
	for _, game := range subject {
		assert.NotEmpty(t, game.Title)
		assert.NotEmpty(t, game.Link)
		assert.NotEmpty(t, game.OldPrice)
		assert.NotEmpty(t, game.Price)
	}
}
