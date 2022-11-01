package writer

import (
	"fmt"
	"testing"

	gotolkit "github.com/dominikus1993/go-toolkit"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"github.com/stretchr/testify/assert"
)

var fakeError = fmt.Errorf("Error")

type fakeOkXboxGameWriter struct {
	executed bool
	records  int
}

func (writer *fakeOkXboxGameWriter) Write(games <-chan data.XboxStoreGame) error {
	for range games {
		writer.records += 1
		writer.executed = true
	}
	return nil
}

type fakeErrorXboxGameWriter struct {
	executed bool
}

func (writer *fakeErrorXboxGameWriter) Write(games <-chan data.XboxStoreGame) error {
	for range games {
		writer.executed = true
	}
	return fakeError
}

func TestBroadcastingGames(t *testing.T) {
	fake := &fakeOkXboxGameWriter{}
	writer := NewBroadcastXboxGameWriter(fake)
	games := []data.XboxStoreGame{{Title: "test"}, {Title: "test2"}}
	err := writer.Write(gotolkit.FromSlice(games))
	assert.NoError(t, err)
	assert.True(t, fake.executed)
	assert.Equal(t, 2, fake.records)
}

func TestBroadcastingGamesWhenOneReturnError(t *testing.T) {
	fake := &fakeOkXboxGameWriter{}
	errorFake := &fakeErrorXboxGameWriter{}
	writer := NewBroadcastXboxGameWriter(fake, errorFake)
	games := []data.XboxStoreGame{{Title: "test"}, {Title: "test2"}}
	err := writer.Write(gotolkit.FromSlice(games))
	assert.Error(t, err)
	assert.True(t, fake.executed)
	assert.Equal(t, 2, fake.records)
	assert.True(t, errorFake.executed)
}
