from typing import Iterable
from bs4 import BeautifulSoup
import requests
from core.parser.xbox import XboxGame, XboxStoreParser


class HtmlXboxStoreParser(XboxStoreParser):
    _xbox_url = "https://www.microsoft.com/pl-pl/store/deals/games/xbox"

    def parse(self) -> Iterable[XboxGame]:
        html = requests.get(self._xbox_url)
        soup = BeautifulSoup(html.text, "html.parser")
        items = soup.find_all("div", {"class": "m-channel-placement-item"})
        for item in items:
            link = item.find("a")
            image = link.find("div", {"class": "c-channel-placement-image"}).find("picture").find("img").get("src")
            product_placement = item.find("div", {"class": "c-channel-placement-content"})
            title = product_placement.find("h3", {"class": "c-subheading-6"}).text
            yield XboxGame(title, link.get("href"), image, 3, 3)
