from typing import Callable, Iterable

from core.parser.xbox import XboxGame

class XboxStorePromotionsCheckService:
    __parse: Callable[[], list[XboxGame]]
    __game_names: list[str]

    def __init__(self, game_names: list[str], parser: Callable[[], list[XboxGame]]):
        self.__parse = parser
        self.__game_names = game_names

    def __check_name(self, game: XboxGame) -> bool:
        return game.title in self.__game_names

    def __check_is_big_promotion(self, game: XboxGame) -> bool:
        return game.count_discount() > 50

    def check_promotions(self) -> Iterable[XboxGame]:
        games = self.__parse()
        for game in games:
            if self.__check_is_big_promotion(game) or self.__check_name(game):
                yield game

