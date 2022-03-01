
import asyncio
import logging
import pprint
from core.services.provider import filter_gamaes, map_ratings
from ratings.metacritic import MetacriticRatingProvider
from parser.html import XboxStoreParser

logging.basicConfig(level=logging.DEBUG, format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')

async def main():
    parser = XboxStoreParser(MetacriticRatingProvider())
    logging.info("Start")
    games_i_have =  ["Cyberpunk 2077", "Red Dead Redemption 2", "Wiedźmin 3: Dziki Gon", "Wiedźmin 3: Dziki Gon – Edycja Gry Roku", "The Witcher 2", "Darksiders Warmastered Edition"]
    games = await parser.parse_all()
    filter_games = filter_gamaes(["Biomutant", "Elden Ring", "Elex 2"], games_i_have, lambda x: x.is_big_promotion() and x.is_ok_game())
    for game in filter_games(games):
        pprint.pprint(game)
    logging.info("Finish")


asyncio.run(main())