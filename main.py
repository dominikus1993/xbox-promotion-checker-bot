
from core.services.provider import check_promotions
from core.usecase.parse import ParseXboxPricesAndNotify
from infrastructure.parser.html import parse_all

promotions = check_promotions(parse_all, ["Cyberpunk 2077"])

for game in promotions:
    print(game)