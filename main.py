
import logging
import pprint
from core.services.provider import filter_gamaes
from infrastructure.parser.html import parse_all
from metacritic.metacritic import is_ok_game

logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')
games = parse_all()
filter_games = filter_gamaes(["Biomutant", "Elden Ring"], ["Cyberpunk 2077", "Red Dead Redemption 2"], lambda x: x.is_big_promotion() and is_ok_game(x))
logging.info("Start")
for game in filter_games(games):
    pprint.pprint(game)

logging.info("Finish")