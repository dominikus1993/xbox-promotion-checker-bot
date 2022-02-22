
from core.services.provider import check_promotions
from infrastructure.parser.html import parse_all
from metacritic.metacritic import check_rating

checker = check_promotions(parse_all, ["Cyberpunk 2077"])
print("Start")
for game in checker():
    rating = check_rating(game)
    if rating is not None and rating.user_rating > 8:
        print(check_rating(game))

print("Finish")