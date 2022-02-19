from abc import ABC, abstractmethod
from dataclasses import dataclass
from typing import Iterable


@dataclass
class XboxGame:
    title: str
    link: str
    image: str
    price_regular: float
    promotion_price: float

class XboxStoreParser(ABC):
    @abstractmethod
    def parse(self) -> Iterable[XboxGame]:
        pass