from abc import ABC, abstractmethod
from dataclasses import dataclass

@dataclass
class XboxGame:
    title: str
    price: str
    link: str
    image: str
    price_regular: float
    promotion_price: float

class XboxStoreParser(ABC):
    @abstractmethod
    def parse(self) -> list[XboxGame]:
        pass