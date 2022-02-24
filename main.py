
import logging
import pprint
from core.services.provider import filter_gamaes, map_ratings
from infrastructure.parser.html import parse_all
from metacritic.metacritic import check_rating

logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')
games_i_have =  ["Cyberpunk 2077", "Red Dead Redemption 2", "Wiedźmin 3: Dziki Gon", "Wiedźmin 3: Dziki Gon – Edycja Gry Roku", "The Witcher 2"]
xbox_games = parse_all()
games = map_ratings(xbox_games, lambda x: check_rating(x))
filter_games = filter_gamaes(["Biomutant", "Elden Ring"], games_i_have, lambda x: x.is_big_promotion() and x.is_ok_game())
logging.info("Start")
for game in filter_games(games):
    pprint.pprint(game)
logging.info("Finish")