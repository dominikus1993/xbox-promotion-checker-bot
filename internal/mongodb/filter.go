package mongo

import (
	"github.com/dominikus1993/go-toolkit/channels"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
)

type DatabaseOldPromotionFilter struct {
	client *MongoClient
}

func NewDatabaseOldPromotionFilter(client *MongoClient) *DatabaseOldPromotionFilter {
	return &DatabaseOldPromotionFilter{client: client}
}

func (f *DatabaseOldPromotionFilter) Filter(games <-chan data.XboxStoreGame) <-chan data.XboxStoreGame {
	return channels.Filter(games, filterGameThatWasInPromtoionSinceWeek(f.client), 10)
}

func filterGameThatWasInPromtoionSinceWeek(client *MongoClient) func(game data.XboxStoreGame) bool {
	return func(game data.XboxStoreGame) bool {
		return true
	}
}
