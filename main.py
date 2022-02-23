
import logging
import pprint
from core.services.provider import get_games_in_promotions
from infrastructure.parser.html import parse_all
from metacritic.metacritic import check_rating

logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')
checker = get_games_in_promotions(parse_all, ["Biomutant", "Elden Ring"], ["Cyberpunk 2077", "Red Dead Redemption 2"])
logging.info("Start")
for game in checker():
    rating = check_rating(game)
    if rating is not None and rating.is_ok_game():
        pprint.pprint(check_rating(game))

logging.info("Finish")