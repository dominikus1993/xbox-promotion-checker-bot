from dataclasses import dataclass
from re import sub
import unittest
from core.parser.xbox import XboxGame
from core.services.provider import check_promotions
@dataclass
class _TestData:
    regular: float
    promotion: float
    discount: float

def fake_parse():
    yield XboxGame("title1", "link", "image", 100, 50)
    yield XboxGame("title2", "link", "image", 100, 90)
    yield XboxGame("title3", "link", "image", 100, 10)
    yield XboxGame("title4", "link", "image", None, None)
    yield XboxGame("title5", "link", "image", None, None)

class TestXboxStorePromotionsCheckService(unittest.TestCase):
    def test_checking_promotion(self):
        subject = check_promotions(fake_parse, ["title1", "title2"])()
        self.assertEqual(list(subject), [XboxGame("title1", "link", "image", 100, 50), XboxGame("title2", "link", "image", 100, 90), XboxGame("title3", "link", "image", 100, 10)])