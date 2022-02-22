
import logging
import pprint
from core.services.provider import check_promotions
from infrastructure.parser.html import parse_all
from metacritic.metacritic import check_rating

logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(name)s - %(levelname)s - %(message)s')
checker = check_promotions(parse_all, ["Cyberpunk 2077"])
logging.info("Start")
for game in checker():
    rating = check_rating(game)
    if rating is not None and rating.user_rating > 8:
        pprint.pprint(check_rating(game))

logging.info("Finish")