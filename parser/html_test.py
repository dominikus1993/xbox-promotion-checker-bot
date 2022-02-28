import unittest
from core.data.game import GameRating, XboxGame

from parser.html import GameRatingProvider, XboxStoreParser


class FakeRatingProvider(GameRatingProvider):
    def provide_rating(self, game: XboxGame) -> GameRating | None:
        return None 



class TestHtmlXboxStoreParser(unittest.TestCase):
    def test_parse(self):
        parser = XboxStoreParser(FakeRatingProvider())
        games = parser.parse()
        self.assertGreater(len(games), 0)
        for igame in games:
            with self.subTest(game=igame):
                self.assertIsNotNone(igame)
                self.assertIsNot(igame.title, "")
                self.assertIsNot(igame.link, "")
                self.assertIsNot(igame.image, "")
                self.assertIsNot(igame.old_price, 0)
                self.assertIsNot(igame.price, 0)
                self.assertIsNotNone(igame.old_price)
                self.assertIsNotNone(igame.price)

    def test_parse_all(self):
        parser = XboxStoreParser(FakeRatingProvider())
        games = parser.parse_all()
        self.assertGreater(len(games), 0)
        for igame in games:
            with self.subTest(game=igame):
                self.assertIsNotNone(igame)
                self.assertIsNot(igame.title, "")
                self.assertIsNot(igame.link, "")
                self.assertIsNot(igame.image, "")
                self.assertIsNot(igame.old_price, 0)
                self.assertIsNot(igame.price, 0)
                self.assertIsNotNone(igame.old_price)
                self.assertIsNotNone(igame.price)