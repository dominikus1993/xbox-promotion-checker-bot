package data

import (
	"testing"

	"github.com/stretchr/testify/assert"
)

func TestParsingFirstPage(t *testing.T) {
	data := []struct {
		game     XboxStoreGame
		expected float64
	}{
		{game: NewXboxStoreGame("", "", 10.0, 20.0), expected: 50},
		{game: NewXboxStoreGame("", "", 10.0, 10.0), expected: 0},
		{game: NewXboxStoreGame("", "", 10.0, 100.0), expected: 90},
	}

	for _, d := range data {
		result := d.game.CalculatePromotionPercentage()
		assert.Equal(t, d.expected, result)
	}
}

func TestGetLink(t *testing.T) {

	link := "pl-pl/p/biomutant-mercenary-class/9pmf2h8q973f%3Fcid=msft_web_chart"
	game := NewXboxStoreGame("", link, 10.0, 20.0)
	subject, err := game.GetLink()

	assert.NoError(t, err)

	assert.Equal(t, "https://www.microsoft.com/pl-pl/p/biomutant-mercenary-class/9pmf2h8q973f%3Fcid=msft_web_chart", subject)
}