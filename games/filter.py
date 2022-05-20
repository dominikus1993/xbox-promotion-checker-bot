

from data.game import Game
from games.loader import GamesLoader
from services.games import GameFilter


class CsvGamesFilter(GameFilter):
    games_that_i_want = []
    def __init__(self, loader: GamesLoader) -> None:
        self.games_that_i_want = loader.load()

    def predicate(self, game: Game) -> bool:
        if not game.is_big_promotion():
            return False

        normalized_name = game.get_normalized_title()
        exists = False
        for g in self.games_that_i_want :
            if g in normalized_name:
                exists = True
                break
        return exists