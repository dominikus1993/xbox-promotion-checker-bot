package html

import (
	"context"
	"testing"

	gotolkit "github.com/dominikus1993/go-toolkit"
	"github.com/stretchr/testify/assert"
)

func TestParsingFirstPage(t *testing.T) {
	parser := XboxStoreHtmlParser{xboxStoreUrl: "https://www.microsoft.com/pl-pl/store/deals/games/xbox", collector: NewCollyCollector()}
	result := parser.parsePage(context.Background(), 1)
	subject := gotolkit.ToSlice(result)
	assert.NotNil(t, subject)
	assert.NotEmpty(t, subject)
	for _, game := range subject {
		link, err := game.GetLink()
		assert.NoError(t, err)
		assert.NotEmpty(t, game.Title)
		assert.NotEmpty(t, link)
		oldprice := game.GetOldPrice()
		assert.NotEmpty(t, oldprice)
		price := game.GetOldPrice()

		assert.NotEmpty(t, price)
	}
}

func TestParsingSecondPage(t *testing.T) {
	parser := XboxStoreHtmlParser{xboxStoreUrl: "https://www.microsoft.com/pl-pl/store/deals/games/xbox", collector: NewCollyCollector()}
	result := parser.parsePage(context.Background(), 2)
	subject := gotolkit.ToSlice(result)
	assert.NotNil(t, subject)
	assert.NotEmpty(t, subject)
	for _, game := range subject {
		link, err := game.GetLink()
		assert.NoError(t, err)
		assert.NotEmpty(t, game.Title)
		assert.NotEmpty(t, link)
		oldprice := game.GetOldPrice()
		assert.NotEmpty(t, oldprice)
		price := game.GetOldPrice()

		assert.NotEmpty(t, price)
	}
}

func TestGetSecondPageUrl(t *testing.T) {
	parser := XboxStoreHtmlParser{xboxStoreUrl: "https://www.microsoft.com/pl-pl/store/deals/games/xbox"}
	subject := parser.getXboxPageUrl(2)
	assert.NotNil(t, subject)
	assert.NotEmpty(t, subject)
	assert.Equal(t, "https://www.microsoft.com/pl-pl/store/deals/games/xbox?skipitems=90", subject)
}

func TestParsingAllPages(t *testing.T) {
	parser := XboxStoreHtmlParser{xboxStoreUrl: "https://www.microsoft.com/pl-pl/store/deals/games/xbox", collector: NewCollyCollector()}
	result := parser.Provide(context.Background())
	subject := gotolkit.ToSlice(result)
	assert.NotNil(t, subject)
	assert.NotEmpty(t, subject)
	for _, game := range subject {
		link, err := game.GetLink()
		assert.NoError(t, err)
		assert.NotEmpty(t, game.Title)
		assert.NotEmpty(t, link)
		oldprice := game.GetOldPrice()
		assert.NotEmpty(t, oldprice)
		price := game.GetOldPrice()
		assert.NotEmpty(t, price)
	}
}
