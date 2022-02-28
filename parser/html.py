from abc import ABC, abstractmethod
from asyncio.log import logger
import itertools
import logging
from multiprocessing import Pool
from multiprocessing import Queue
from typing import Any, Iterable
from bs4 import BeautifulSoup
import requests
from core.data.game import Game, GameRating, XboxGame

class GameRatingProvider(ABC):
    @abstractmethod
    def provide_rating(self, game: XboxGame) -> GameRating | None:
        pass

class XboxStoreParser:
    __provider: GameRatingProvider

    def __init__(self, provider: GameRatingProvider):
        self.__provider = provider

    __CURRENCY = "zł"
    __XBOX_URL = "https://www.microsoft.com/pl-pl/store/deals/games/xbox"
    
    def __try_parse_float(self, value: str) -> float | None:
        try:
            return float(value)
        except:
            return None
    
    def __get_xbox_url(self, page: int) -> str: 
        if page == 1: 
            return self.__XBOX_URL
        return f"{self.__XBOX_URL}?s=store&skipitems={(page - 1) * 90}"
    
    def __parse_price(self, price_placement: Any) -> float | None:
        price = price_placement.find("span", {"itemprop": "price"}).get("content")
        if price == "" or price is None:
            return None
        return self.__try_parse_float(price.replace(",", "."))
    
    def __parse_old_price(self, price_placement: Any, currency: str) -> float | None:
        price = price_placement.find("s")
        if price == "" or price is None:
            return None
        return self.__try_parse_float(price.text.replace(currency, "").replace(",", ".").strip())
    
    def __create_xbox_game(self, item: Any) -> XboxGame | None:
        product_placement = item.find("div", {"class": "c-channel-placement-content"})
        price_placement = product_placement.find("div", {"class": "c-channel-placement-price"})
        prices = price_placement.find("div", {"class": "c-price"})
        price = self.__parse_price(prices)
        old_price = self.__parse_old_price(prices, self.__CURRENCY)
    
        if price is None or old_price is None:
            return None
    
        link_element = item.find("a")
        image = link_element.find("div", {"class": "c-channel-placement-image"}).find("picture").find("img").get("src")
        link = link_element.get("href").replace("/p/", "/games/store/")
        title = product_placement.find("h3", {"class": "c-subheading-6"}).text
        return XboxGame(title, f"https://www.xbox.com{link}", image, old_price, price)
    
    def __parse(self, url: str, ):
        print(f"Parsing {url}")
        html = requests.get(url)
        soup = BeautifulSoup(html.text, "html.parser")
        items = soup.find_all("div", {"class": "m-channel-placement-item"})
        if items is None or len(items) == 0:
            return []
        
        result: list[Game] = []
        for item in items:
            xbox_game = self.__create_xbox_game(item)
            if xbox_game is not None:
                rating = self.__provider.provide_rating(xbox_game)
                game = Game.from_xbox_game(xbox_game, rating)
                result.append(game)
        return result

    def __parse_all(self, q: Queue): 
        

    def parse_all(self) -> list[Game]:
        with Pool(processes=4) as pool:
            result = pool.map(self.__parse, itertools.repeat(self.__XBOX_URL, 4))
            return list(itertools.chain.from_iterable(result))
