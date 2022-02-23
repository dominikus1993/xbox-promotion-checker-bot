from abc import ABC, abstractmethod
from dataclasses import dataclass
from typing import Iterable
from core.utils.price import count_discount


@dataclass
class XboxGame:
    title: str
    link: str
    image: str
    old_price: float | None
    price: float | None

    def count_discount(self):
        if self.old_price is None:
            return 0
        if self.price is None:
            return 0
        return count_discount(self.old_price, self.price)

    def is_big_promotion(self):
        return self.count_discount() > 70

class XboxStoreParser(ABC):
    @abstractmethod
    def parse(self) -> Iterable[XboxGame]:
        pass