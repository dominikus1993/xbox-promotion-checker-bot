import unittest

from infrastructure.parser.html import HtmlXboxStoreParser


class TestHtmlXboxStoreParser(unittest.TestCase):
    def test_parser(self):
        parser = HtmlXboxStoreParser()
        games = [item for item in parser.parse()]
        self.assertGreater(len(games), 0)
        for game in games:
            self.assertIsNotNone(game)
            self.assertIsNot(game.title, "")
            self.assertIsNot(game.link, "")
            self.assertIsNot(game.image, "")
            self.assertIsNot(game.price_regular, 0)
            self.assertIsNot(game.promotion_price, 0)