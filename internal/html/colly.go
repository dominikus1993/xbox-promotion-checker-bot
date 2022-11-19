package html

import (
	"time"

	"github.com/gocolly/colly/v2"
)

const userAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/605.1.15 (KHTML, like Gecko) Version/16.1 Safari/605.1.15"

func NewCollyCollector() *colly.Collector {
	c := colly.NewCollector(colly.UserAgent(userAgent))
	c.Limit(&colly.LimitRule{
		Parallelism: 2,
		RandomDelay: 5 * time.Second,
	})

	return c
}
