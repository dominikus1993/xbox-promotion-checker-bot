
from abc import ABC, abstractmethod


class GamesLoader(ABC):
    @abstractmethod
    def load(self) -> list[str]:
        pass

class TxtGamesLoader(GamesLoader):
    path: str

    def __init__(self, path: str) -> None:
        self.path = path

    def load(self) -> list[str]:
        with open(self.path, "r") as f:
            games = f.read().splitlines()
            return [game.lower() for game in games]