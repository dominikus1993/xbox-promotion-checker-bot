
import asyncio
import logging
import pprint
from config.file import load_games_file
from data.game import Game
from games.filter import CsvGamesFilter
from games.html import XboxStoreParser
from services.games import CheckXboxGamePromotionsService, Writer

logging.basicConfig(level=logging.DEBUG, format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')

class ConsoleWriter(Writer):
    def write(self, game: Game) -> None:
        pprint.pprint(game)


async def main():
    games_to_parse = load_games_file()
    parser = XboxStoreParser()
    writer = ConsoleWriter()
    games_filter = CsvGamesFilter(games_to_parse)
    service = CheckXboxGamePromotionsService(parser, writer, games_filter)
    logging.info("Start")
    await service.execute()
    logging.info("Finish")


asyncio.run(main())
