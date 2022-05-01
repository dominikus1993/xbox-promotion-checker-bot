
import asyncio
import logging
import pprint
from config.file import load_games_file
from ratings.metacritic import MetacriticRatingProvider
from games.html import XboxStoreParser

logging.basicConfig(level=logging.DEBUG, format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')

async def main():
    parser = XboxStoreParser()
    logging.info("Start")
    games_to_parse = load_games_file()
    games = await parser.parse_all()
    filter_games = [game for game in games if game.title in games_to_parse]
    for game in filter_games:
        pprint.pprint(game)
    logging.info("Finish")


asyncio.run(main())
