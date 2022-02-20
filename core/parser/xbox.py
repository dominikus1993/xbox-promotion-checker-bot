from abc import ABC, abstractmethod
from dataclasses import dataclass
from typing import Iterable


@dataclass
class XboxGame:
    title: str
    link: str
    image: str
    old_price: float | None
    price: float | None

class XboxStoreParser(ABC):
    @abstractmethod
    def parse(self) -> Iterable[XboxGame]:
        pass