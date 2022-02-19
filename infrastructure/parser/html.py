from core.parser.xbox import XboxGame, XboxStoreParser


class HtmlXboxStoreParser(XboxStoreParser):
    def parse(self) -> list[XboxGame]:
        return []