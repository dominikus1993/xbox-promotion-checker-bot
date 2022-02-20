import unittest

from infrastructure.parser.html import parse, parse_all


class TestHtmlXboxStoreParser(unittest.TestCase):
    def test_parse(self):
        games = [item for item in parse()]
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
        games = [item for item in parse_all()]
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