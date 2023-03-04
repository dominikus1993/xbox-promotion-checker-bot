output_file = xbox-promotion-checker-bot
webhookToken = ${DISCORD_WEBHOOK_TOKEN} 
webhookId = ${DISCORD_WEBHOOK_ID}
mongoConnection = ${MONGO_CONNECTION_STRING}
build:
	go build -o $(output_file)

test:
	go test ./...

vet: 
	go vet ./...
	
buildandtest: build test

run:
	go run .

upgrade:
	go get -u
	
runbin: build
	./$(output_file) --webhooktoken "$(strip $(webhookToken))" --webhookid "$(strip $(webhookId))" --mongo-connection "$(strip $(mongoConnection))"