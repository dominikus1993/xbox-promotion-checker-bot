package discord

import (
	"fmt"

	"github.com/bwmarrin/discordgo"
	"github.com/dominikus1993/xbox-promotion-checker-bot/pkg/data"
	log "github.com/sirupsen/logrus"
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
		webhookID:    webhookID,
		webhookToken: webhookToken,
		client:       session,
	}, nil
}

func (w *DiscordXboxGameWriter) Write(games <-chan data.XboxStoreGame) error {
	embeds := make([]*discordgo.MessageEmbed, 0)
	for game := range games {
		link, err := game.GetLink()
		if err != nil {
			return err
		}
		embeds = append(embeds, &discordgo.MessageEmbed{
			Title:       game.Title,
			Description: fmt.Sprintf("Witam gra potaniala z %s do %s co daje promke %f procent", game.GetOldPrice(), game.GetPrice(), game.CalculatePromotionPercentage()),
			URL:         link,
			Color:       0x00ff00,
		})
	}
	if len(embeds) == 0 {
		log.Infoln("no games to send")
		return nil
	}

	msg := discordgo.WebhookParams{Content: "Witam serdecznie, oto nowe gry w promocji", Embeds: embeds}
	_, err := w.client.WebhookExecute(w.webhookID, w.webhookToken, true, &msg)
	if err != nil {
		return fmt.Errorf("error while sending webhook: %w", err)
	}

	return nil
}
