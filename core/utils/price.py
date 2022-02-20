

def count_discount(regular_price: float, promotion_price: float) -> float:
    if regular_price == 0:
        return 0
    if regular_price == 0 and promotion_price == 0:
        return 0
    if promotion_price == 0:
        return 100
    return (regular_price - promotion_price) * 100 / regular_price