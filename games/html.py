import asyncio
import itertools
import logging
from typing import Any
import aiohttp
from bs4 import BeautifulSoup
from data.game import Game, XboxGame

class XboxStoreParser:
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
        if link_element is None:
            return None
        image_div = link_element.find("div", {"class": "c-channel-placement-image"})
        if image_div is None:
            return None
        picture_element = image_div.find("picture")
        if picture_element is None:
            return None
        image = picture_element.find("img").get("src")
        link = link_element.get("href").replace("/p/", "/games/store/")
        title = product_placement.find("h3", {"class": "c-subheading-6"}).text
        return XboxGame(title, f"https://www.xbox.com{link}", image, old_price, price)
    
    async def __parse(self, url: str, session: aiohttp.ClientSession):
        logging.info(f"Parsing {url}")
        async with session.get(url) as response:
            soup = BeautifulSoup(await response.text(), "html.parser")
            items = soup.find_all("div", {"class": "m-channel-placement-item"})
            if items is None or len(items) == 0:
                return []
            games = [x for x in map(self.__create_xbox_game, items) if x is not None]
            return [Game.from_xbox_game(game) for game in games]


    async def parse_all(self) -> list[Game]:
        logging.info('Parsing all games')
        async with aiohttp.ClientSession() as session:
            tasks = [t for t in [self.__parse(self.__get_xbox_url(i), session) for i in range(1, 7)] if t is not None]
            results = await asyncio.gather(*tasks)
            return list(itertools.chain(*results))
