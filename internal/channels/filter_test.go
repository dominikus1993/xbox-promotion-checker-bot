package channels

import (
	"testing"

	"github.com/dominikus1993/go-toolkit/channels"
	"github.com/stretchr/testify/assert"
)

func rangeInt(from, to int) []int {
	res := make([]int, 0, to-from)
	for i := from; i < to; i++ {
		res = append(res, i)
	}
	return res
}

func TestFilter(t *testing.T) {
	numbers := channels.FromSlice(rangeInt(1, 10))
	result := Filter(numbers, func(element int) bool { return element%2 == 0 }, 10)
	subject := channels.ToSlice(result)
	assert.Len(t, subject, 4)
	assert.ElementsMatch(t, []int{2, 4, 6, 8}, subject)
}

func BenchmarkFilter(b *testing.B) {
	for n := 0; n < b.N; n++ {
		numbers := channels.FromSlice(rangeInt(1, 10))
		channels.ToSlice(Filter(numbers, func(element int) bool { return element%2 == 0 }, 1))
	}
}
