import unittest
from core.parser.xbox import XboxGame

class TestXboxGameModel(unittest.TestCase):
    def test_is_big_promotion(self):
        subject =  XboxGame("title1", "link", "image", 100, 15).is_big_promotion()
        self.assertEqual(subject, True)