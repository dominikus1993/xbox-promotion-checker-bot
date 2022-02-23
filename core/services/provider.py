from typing import Callable, Iterable
from core.parser.xbox import XboxGame

def filter_gamaes(obseved_games: list[str], games_i_have: list[str], predicate: Callable[[XboxGame], bool]):
    def checker(games: Iterable[XboxGame]) -> Iterable[XboxGame]:
        for game in games:
            if (game.title in obseved_games or predicate(game)) and game.title not in games_i_have:
                yield game
    return checker