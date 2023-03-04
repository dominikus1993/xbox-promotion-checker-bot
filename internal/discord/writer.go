package discord

import (
	"context"
	"errors"
	"fmt"
	"strings"

	"github.com/bwmarrin/discordgo"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	"github.com/samber/lo"
)

type DiscordXboxGameWriter struct {
	webhookID    string
	webhookToken string
	client       *discordgo.Session
}

func NewDiscordXboxGameWriter(webhookID, webhookToken string) (*DiscordXboxGameWriter, error) {
	session, err := discordgo.New("")
	if err != nil {
		return nil, err
	}
	return &DiscordXboxGameWriter{
		webhookID:    strings.TrimSpace(webhookID),
		webhookToken: strings.TrimSpace(webhookToken),
		client:       session,
	}, nil
}

func createEmbeds(games []data.XboxStoreGame) ([]*discordgo.MessageEmbed, error) {
	embeds := make([]*discordgo.MessageEmbed, len(games))
	for i, game := range games {
		link, err := game.GetLink()
		if err != nil {
			return nil, err
		}

		embeds[i] = &discordgo.MessageEmbed{
			Title:       game.Title,
			Description: fmt.Sprintf("Witam gra potaniala z %s do %s co daje promke %s procent", game.FormatOldPrice(), game.FormatPrice(), game.FormatPromotionPercentage()),
			URL:         link.String(),
			Color:       0x00ff00,
		}
	}
	return embeds, nil
}

func (w *DiscordXboxGameWriter) Write(ctx context.Context, games []data.XboxStoreGame) error {
	var result error

	embeds, result := createEmbeds(games)

	uniqueEmbeds := lo.UniqBy(embeds, func(embed *discordgo.MessageEmbed) string { return embed.URL })
	chunks := lo.Chunk(uniqueEmbeds, 9)
	for _, chunkE := range chunks {
		msg := discordgo.WebhookParams{Content: "Witam serdecznie, oto nowe gry w promocji", Embeds: chunkE}
		_, err := w.client.WebhookExecute(w.webhookID, w.webhookToken, true, &msg)
		if err != nil {
			result = errors.Join(result, fmt.Errorf("error while sending webhook: %w", err))
		}
	}
	return result
}
