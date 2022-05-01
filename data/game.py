from dataclasses import dataclass
from utils.price import count_discount


@dataclass
class XboxGame:
    title: str
    link: str
    image: str
    old_price: float | None
    price: float | None


@dataclass
class Game: 
    title: str
    link: str
    image: str
    old_price: float | None
    price: float | None

    @staticmethod
    def from_xbox_game(xbox_game: XboxGame) -> 'Game':
        return Game(xbox_game.title, xbox_game.link, xbox_game.image, xbox_game.old_price, xbox_game.price)
        
    def count_discount(self):
        if self.old_price is None:
            return 0
        if self.price is None:
            return 0
        return count_discount(self.old_price, self.price)

    def is_big_promotion(self):
        return self.count_discount() > 70