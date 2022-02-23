from typing import Callable, Iterable
from core.parser.xbox import XboxGame


def get_games_in_promotions(parse: Callable[[], Iterable[XboxGame]], game_names: list[str], games_i_have: list[str]) -> Callable[[], Iterable[XboxGame]]:
    def checker() -> Iterable[XboxGame]:
        for game in parse():
            if game.is_big_promotion() or game.title in game_names and game.title not in games_i_have:
                yield game
    return checker