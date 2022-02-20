from dataclasses import dataclass
from tkinter import FLAT
import unittest

from core.utils.price import count_discount

@dataclass
class _TestData:
    regular: float
    promotion: float
    discount: float

class TestHtmlXboxStoreParser(unittest.TestCase):
    def test_parse(self):
        data = [_TestData(100, 50, 50), _TestData(100, 100, 0), _TestData(100, 0, 100), _TestData(0, 0, 0), _TestData(25, 20, 20)]
        for d in data:
            with self.subTest(td=d):
                self.assertEqual(count_discount(d.regular, d.promotion), d.discount)