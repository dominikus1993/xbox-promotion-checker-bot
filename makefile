output_file = xbox-promotion-checker-bot
webhookToken = ${DISCORD_WEBHOOK_TOKEN} 
webhookId = ${DISCORD_WEBHOOK_ID}
test:
	go test ./...

build:
	go build -o $(output_file)

run:
	go run .

runbin: build
	./$(output_file) --webhooktoken "$(webhookToken)" --webhookid "$(webhookId)"