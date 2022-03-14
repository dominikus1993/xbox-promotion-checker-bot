
import asyncio
import logging
import pprint
from config.file import load_games_file
from core.services.provider import filter_gamaes
from ratings.metacritic import MetacriticRatingProvider
from games.html import XboxStoreParser

logging.basicConfig(level=logging.DEBUG, format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')

async def main():
    parser = XboxStoreParser(MetacriticRatingProvider())
    logging.info("Start")
    games_i_have = load_games_file()
    games = await parser.parse_all()
    filter_games = filter_gamaes(["Biomutant", "Elden Ring", "Elex 2"], games_i_have, lambda x: x.is_big_promotion() and x.is_ok_game())
    for game in filter_games(games):
        pprint.pprint(game)
    logging.info("Finish")


asyncio.run(main())
