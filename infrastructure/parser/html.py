from typing import Any, Iterable
from bs4 import BeautifulSoup
import requests
from core.data.game import XboxGame

__CURRENCY = "zł"
__XBOX_URL = "https://www.microsoft.com/pl-pl/store/deals/games/xbox"

def __try_parse_float(value: str) -> float | None:
    try:
        return float(value)
    except:
        return None

def __get_xbox_url(page: int) -> str: 
    if page == 1: 
        return __XBOX_URL
    return f"{__XBOX_URL}?s=store&skipitems={(page - 1) * 90}"

def __parse_price(price_placement: Any) -> float | None:
    price = price_placement.find("span", {"itemprop": "price"}).get("content")
    if price == "" or price is None:
        return None
    return __try_parse_float(price.replace(",", "."))

def __parse_old_price(price_placement: Any, currency: str) -> float | None:
    price = price_placement.find("s")
    if price == "" or price is None:
        return None
    return __try_parse_float(price.text.replace(currency, "").replace(",", ".").strip())

def __create_xbox_game(item: Any) -> XboxGame | None:
    product_placement = item.find("div", {"class": "c-channel-placement-content"})
    price_placement = product_placement.find("div", {"class": "c-channel-placement-price"})
    prices = price_placement.find("div", {"class": "c-price"})
    price = __parse_price(prices)
    old_price = __parse_old_price(prices, __CURRENCY)

    if price is None or old_price is None:
        return None

    link_element = item.find("a")
    image = link_element.find("div", {"class": "c-channel-placement-image"}).find("picture").find("img").get("src")
    link = link_element.get("href").replace("/p/", "/games/store/")
    title = product_placement.find("h3", {"class": "c-subheading-6"}).text
    return XboxGame(title, f"https://www.xbox.com{link}", image, old_price, price)

def __parse(url: str) -> Iterable[XboxGame]:
    html = requests.get(url)
    soup = BeautifulSoup(html.text, "html.parser")
    items = soup.find_all("div", {"class": "m-channel-placement-item"})
    if items is None or len(items) == 0:
        return
    for item in items:
        game = __create_xbox_game(item)
        if game is not None:
            yield game


def parse() -> Iterable[XboxGame]:
    return __parse(__get_xbox_url(1))

def parse_all() -> Iterable[XboxGame]:
    for page in range(1, 7):
        for game in __parse(__get_xbox_url(page)):
            yield game

