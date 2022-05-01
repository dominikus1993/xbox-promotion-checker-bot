
import asyncio
import logging
import pprint
from config.file import load_games_file
from games.html import XboxStoreParser

logging.basicConfig(level=logging.DEBUG, format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')

def search_title(games: list[str],)

async def main():
    parser = XboxStoreParser()
    logging.info("Start")
    games_to_parse = load_games_file()
    pprint.pprint(games_to_parse)
    games = await parser.parse_all()
    filter_games = [game for game in games if game.get_normalized_title() in games_to_parse]
    for game in filter_games:
        pprint.pprint(game)
    logging.info("Finish")


asyncio.run(main())
