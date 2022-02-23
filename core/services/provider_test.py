from dataclasses import dataclass
from re import sub
import unittest
from core.parser.xbox import XboxGame
from core.services.provider import filter_gamaes

def fake_parse():
    yield XboxGame("title1", "link", "image", 100, 50)
    yield XboxGame("title2", "link", "image", 100, 90)
    yield XboxGame("title3", "link", "image", 100, 10)
    yield XboxGame("title4", "link", "image", None, None)
    yield XboxGame("title5", "link", "image", None, None)
    yield XboxGame("title6", "link", "image", 100, 50)

class TestXboxStorePromotionsCheckService(unittest.TestCase):
    def test_checking_promotion(self):
        subject = filter_gamaes(["title1", "title2"], ["title6"], lambda x: x.is_big_promotion())(fake_parse())
        self.assertEqual(list(subject), [XboxGame("title1", "link", "image", 100, 50), XboxGame("title2", "link", "image", 100, 90), XboxGame("title3", "link", "image", 100, 10)])