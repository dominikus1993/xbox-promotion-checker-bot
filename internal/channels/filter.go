package channels

func Filter[T any](data <-chan T, predicate func(T) bool, size int) <-chan T {
	out := make(chan T, size)
	go func() {
		for d := range data {
			if predicate(d) {
				out <- d
			}
		}
		close(out)
	}()
	return out
}
