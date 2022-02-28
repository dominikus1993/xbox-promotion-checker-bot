from re import A
import unittest
from core.data.game import XboxGame
from ratings.metacritic import MetacriticRatingProvider


class TestHtmlXboxStoreParser(unittest.IsolatedAsyncioTestCase):
    async def test_check_rating(self):
        provider = MetacriticRatingProvider()
        rating = await provider.provide_rating(XboxGame("Red Dead Redemption 2", "", "", 0, 0))
        self.assertIsNotNone(rating)
        assert rating is not None
        self.assertIsNot(rating.user_rating, 0)
        self.assertIsNot(rating.reviews, 0)

    async def test_check_rating_fake_game(self):
        provider = MetacriticRatingProvider()
        rating = await provider.provide_rating(XboxGame("Red Dead Redemption 222", "", "", 0, 0))
        self.assertIsNone(rating)