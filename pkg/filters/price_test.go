package filter

import (
	"context"
	"testing"

	data "github.com/dominikus1993/xbox-promotion-checker-bot/pkg/model"
	"github.com/samber/lo"
	"github.com/stretchr/testify/assert"
)

func TestPriceFilter(t *testing.T) {
	filter := NewPriceFilter(50)
	games := []data.XboxStoreGame{data.NewXboxStoreGame("test", "test", 10, 20), data.NewXboxStoreGame("test", "test", 10, 10), data.NewXboxStoreGame("test", "test", 10, 30)}
	stream := lo.SliceToChannel(10, games)

	subject := lo.ChannelToSlice(filter.Filter(context.TODO(), stream))

	assert.Len(t, subject, 2)
}
