

def load_games_file() -> list[str]:
    with open("games.txt", "r") as f:
        games = f.read().splitlines()
        return [game.lower() for game in games]
