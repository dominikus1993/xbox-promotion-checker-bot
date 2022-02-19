import unittest

from infrastructure.parser.html import HtmlXboxStoreParser


class TestHtmlXboxStoreParser(unittest.TestCase):
    def test_parser(self):
        parser = HtmlXboxStoreParser()
        games = parser.parse()
        self.assertGreater(len(games), 0)