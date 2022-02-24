from dataclasses import dataclass
from bs4 import BeautifulSoup
import requests
from core.data.game import GameRating, XboxGame

def __parse_rating(r: str) -> float:
    if r == "tbd":
        return 0
    return float(r)

def check_rating(game: XboxGame) -> GameRating | None:
    search_term = game.title.lower().replace(" ", "-")
    html = requests.get(f"https://www.metacritic.com/game/xbox-one/{search_term}", headers = {'User-Agent': 'Mozilla/5.0'})

    if html.status_code != 200:
        return None

    soup = BeautifulSoup(html.text, "html.parser")
    res = []
    ratings = soup.find_all("a", {"class": "metascore_anchor"}, recursive=True)

    if len(ratings) == 0:
        return None
        
    for rating_el in ratings:
        if search_term in rating_el.get("href"):
            res.append(rating_el.find("div", {"class": "metascore_w"}).text)

    if len(res) == 2:
        return GameRating(__parse_rating(res[1]), __parse_rating(res[0]))

    return None