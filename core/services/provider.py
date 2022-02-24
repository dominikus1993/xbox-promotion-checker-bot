from typing import Callable, Iterable

from core.data.game import Game, GameRating, XboxGame

def filter_gamaes(obseved_games: list[str], games_i_have: list[str], predicate: Callable[[Game], bool]):
    def checker(games: Iterable[Game]) -> Iterable[Game]:
        for game in games:
            if (game.title in obseved_games or predicate(game)) and game.title not in games_i_have:
                yield game
    return checker

def map_ratings(games: Iterable[XboxGame], mapper: Callable[[XboxGame], GameRating | None]) -> Iterable[Game]:
    for game in games:
        yield Game.from_xbox_game(game, mapper(game))