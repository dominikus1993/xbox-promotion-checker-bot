from dataclasses import dataclass
import unittest

from core.utils.price import count_discount

@dataclass
class _TestData:
    regular: float
    promotion: float
    discount: float

class TestCountDiscount(unittest.TestCase):
    def test_count_discount(self):
        data = [_TestData(100, 50, 50), _TestData(100, 100, 0), _TestData(100, 0, 100), _TestData(0, 0, 0), _TestData(25, 20, 20)]
        for d in data:
            with self.subTest(td=d):
                self.assertEqual(count_discount(d.regular, d.promotion), d.discount)