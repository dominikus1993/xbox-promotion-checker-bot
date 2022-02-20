import os
from typing import Any, Iterable
from bs4 import BeautifulSoup
import requests
from core.parser.xbox import XboxGame, XboxStoreParser
from multiprocessing import Pool

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

    link = item.find("a")
    image = link.find("div", {"class": "c-channel-placement-image"}).find("picture").find("img").get("src")
    title = product_placement.find("h3", {"class": "c-subheading-6"}).text
    return XboxGame(title, link.get("href"), image, old_price, price)

def __parse(url: str) -> list[XboxGame]:
    html = requests.get(url)
    soup = BeautifulSoup(html.text, "html.parser")
    items = soup.find_all("div", {"class": "m-channel-placement-item"})
    if items is None or len(items) == 0:
        return []
    result = []
    for item in items:
        game = __create_xbox_game(item)
        if game is not None:
            result.append(game)
    
    return result

def parse() -> list[XboxGame]:
    return __parse(__get_xbox_url(1))

def parse_all():
    with Pool(os.cpu_count()) as p:
        games = p.map(__parse, [__get_xbox_url(page) for page in range(1, 5)])
        return [game for game_list in games for game in game_list]

