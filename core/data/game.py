from dataclasses import dataclass
from core.utils.price import count_discount


@dataclass
class XboxGame:
    title: str
    link: str
    image: str
    old_price: float | None
    price: float | None

@dataclass
class GameRating:
    user_rating: float
    reviews: float

@dataclass
class Game: 
    title: str
    link: str
    image: str
    old_price: float | None
    price: float | None
    user_ratings: float | None
    reviews: float | None

    @staticmethod
    def from_xbox_game(xbox_game: XboxGame, rating: GameRating | None) -> 'Game':
        if rating is None:
            return Game(xbox_game.title, xbox_game.link, xbox_game.image, xbox_game.old_price, xbox_game.price, None, None)
        return Game(xbox_game.title, xbox_game.link, xbox_game.image, xbox_game.old_price, xbox_game.price, rating.user_rating, rating.reviews)

    def is_ok_game(self) -> bool:
        if self.user_ratings is None or self.reviews is None:
            return True
        return self.user_ratings > 8 or self.reviews > 80

    def count_discount(self):
        if self.old_price is None:
            return 0
        if self.price is None:
            return 0
        return count_discount(self.old_price, self.price)

    def is_big_promotion(self):
        return self.count_discount() > 70