package writer

import "github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"

type XboxGameWriter interface {
	Write(games <-chan data.XboxStoreGame) error
}
