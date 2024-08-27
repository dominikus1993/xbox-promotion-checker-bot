package html

import (
	"time"

	"github.com/gocolly/colly/v2"
)

const userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/127.0.0.0 Safari/537.36 Edg/127.0.0.0"

func NewCollyCollector() *colly.Collector {
	c := colly.NewCollector(colly.UserAgent(userAgent), colly.Async(true))
	c.Limit(&colly.LimitRule{
		Parallelism: 2,
		RandomDelay: 5 * time.Second,
	})

	return c
}
