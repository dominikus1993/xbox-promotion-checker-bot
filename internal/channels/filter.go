package channels

func Filter[T any](data <-chan T, predicate func(T) bool) <-chan T {
	out := make(chan T)
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
