import unittest
from core.data.game import Game, XboxGame
from core.services.provider import filter_gamaes

def fake_parse():
    yield Game("title1", "link", "image", 100, 50, None, None)
    yield Game("title2", "link", "image", 100, 90, None, None)
    yield Game("title3", "link", "image", 100, 10, None, None)
    yield Game("title4", "link", "image", None, None, None, None)
    yield Game("title5", "link", "image", None, None, None, None)
    yield Game("title6", "link", "image", 100, 50, None, None)

class TestXboxStorePromotionsCheckService(unittest.TestCase):
    def test_checking_promotion(self):
        subject = filter_gamaes(["title1", "title2"], ["title6"], lambda x: x.is_big_promotion())(fake_parse())
        self.assertEqual(list(subject), [Game("title1", "link", "image", 100, 50, None, None), Game("title2", "link", "image", 100, 90, None, None), Game("title3", "link", "image", 100, 10, None, None)])