
from data.game import Game, XboxGame
from abc import ABC, abstractmethod

from games.html import XboxStoreParser


class Writer(ABC):
    @abstractmethod
    def write(self, game: Game) -> None:
        pass

class GameFilter(ABC):
    @abstractmethod
    def predicate(self, game: Game) -> bool:
        pass


class CheckXboxGamePromotionsService:
    __parser: XboxStoreParser
    __writer: Writer
    __filter: GameFilter

    def __init__(self, parser: XboxStoreParser, writer: Writer, filter: GameFilter):
        self.__parser = parser
        self.__writer = writer
        self.__filter = filter

        

    async def execute(self): 
        games = await self.__parser.parse_all()
        filter_games = [game for game in games if self.__filter.predicate(game)]
        for game in filter_games:
            self.__writer.write(game)