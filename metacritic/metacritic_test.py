import unittest
from core.parser.xbox import XboxGame

from metacritic.metacritic import check_rating


class TestHtmlXboxStoreParser(unittest.TestCase):
    def test_check_rating(self):
        rating = check_rating(XboxGame("Red Dead Redemption 2", "", "", 0, 0))
        self.assertIsNotNone(rating)
        assert rating is not None
        self.assertIsNot(rating.user_rating, 0)
        self.assertIsNot(rating.reviews, 0)

    def test_check_rating_fake_game(self):
        rating = check_rating(XboxGame("Red Dead Redemption 222", "", "", 0, 0))
        self.assertIsNone(rating)