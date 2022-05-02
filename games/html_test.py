import unittest
from data.game import XboxGame
from unittest import IsolatedAsyncioTestCase
from games.html import XboxStoreParser

class TestHtmlXboxStoreParser(IsolatedAsyncioTestCase):
    async def test_parse(self):
        parser = XboxStoreParser()
        games = await parser.parse_all()
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

    async def test_parse_all(self):
        parser = XboxStoreParser()
        games = await parser.parse_all()
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