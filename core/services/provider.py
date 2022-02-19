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

class XboxStorePromotionsCheckService(ABC):
    @abstractmethod
    def check_promotions(self, game_names: list[str]) -> XboxGame | None:
        pass

