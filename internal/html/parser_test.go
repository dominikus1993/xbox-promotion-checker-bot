package html

import (
	"context"
	"fmt"
	"testing"

	gotolkit "github.com/dominikus1993/go-toolkit/channels"
	"github.com/stretchr/testify/assert"
)

func FuzzParsingPrice(f *testing.F) {
	f.Add("19.47\\u00a0 zz\\", 19.47, false)
	f.Add("21.37zł", 21.37, false)
	f.Add("21.37 zł", 21.37, false)
	f.Add("21,37 zł", 21.37, false)
	f.Fuzz(func(t *testing.T, priceTxt string, expectedPrice float64, isError bool) {
		result, err := parsePrice(priceTxt)
		assert.Equal(t, expectedPrice, result)
		if !isError {
			assert.NoError(t, err)
		} else {
			assert.NotNil(t, err)
		}
	})
}

func TestParsingFirstPage(t *testing.T) {
	parser := XboxStoreHtmlParser{xboxStoreUrl: "https://www.microsoft.com/pl-pl/store/deals/games/xbox", collector: NewCollyCollector()}
	result := parser.parsePage(context.Background(), 1)
	subject := gotolkit.ToSlice(result)
	assert.NotNil(t, subject)
	assert.NotEmpty(t, subject)
	for _, game := range subject {
		link, err := game.GetLink()
		assert.NoError(t, err)
		assert.NotEmpty(t, game.Title, fmt.Sprintf("Game of link %s", link))
		assert.NotEmpty(t, link)
		oldprice := game.FormatOldPrice()
		assert.NotEmpty(t, oldprice)
		price := game.FormatPrice()
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
		oldprice := game.FormatOldPrice()
		assert.NotEmpty(t, oldprice)
		price := game.FormatPrice()

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
		oldprice := game.FormatOldPrice()
		assert.NotEmpty(t, oldprice)
		price := game.FormatPrice()
		assert.NotEmpty(t, price)
	}
}
