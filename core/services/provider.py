from typing import Callable, Iterable
from core.parser.xbox import XboxGame


def __is_big_promotion(game: XboxGame) -> bool:
    return game.count_discount() > 70

def __check_name(names: list[str], game: XboxGame) -> bool:
    return game.title in names

def __is_ok_promotion(names: list[str], game: XboxGame) -> bool:
    return __is_big_promotion(game) or __check_name(names, game)

def check_promotions(parse: Callable[[], Iterable[XboxGame]], game_names: list[str]):
    def checker() -> Iterable[XboxGame]:
        for game in parse():
            if __is_ok_promotion(game_names, game):
                yield game
    return checker