output_file = xbox-promotion-checker-bot
test:
	go test ./...

build:
	go build -o $(output_file)

run:
	go run .

runbin: build
	go run .