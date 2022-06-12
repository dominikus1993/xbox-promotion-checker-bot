package filter

import (
	"testing"

	"github.com/stretchr/testify/assert"
)

func TestParsingFirstPage(t *testing.T) {
	data := []struct {
		price    float64
		oldPrice float64
		expected float64
	}{
		{price: 10.0, oldPrice: 20.0, expected: 50},
		{price: 10.0, oldPrice: 10.0, expected: 0},
		{price: 10.0, oldPrice: 100.0, expected: 90},
	}

	for _, d := range data {
		result := calculatePromotionPercentage(&d.oldPrice, &d.price)
		assert.Equal(t, d.expected, result)
	}
}
