

def load_games_file() -> list[str]:
    with open("games.txt", "r") as f:
        return f.read().splitlines()
