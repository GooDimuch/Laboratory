def solution(number):
	if number < 0: return 0
	number -= 1
	return get_sum_arithmetic_progression(number, 3) + \
	       get_sum_arithmetic_progression(number, 5) - \
	       get_sum_arithmetic_progression(number, 15)


def get_sum_arithmetic_progression(number, base):
	nearest_multiple = get_nearest_multiple(number, base)
	return (base + nearest_multiple) * nearest_multiple / base / 2

def get_nearest_multiple(number, base):
	return number - number % base


# 3 6 9 12 15 18 21 24 27 30 33 36 39 42 45 48
# 5 10 15 20 25 30 35 40 45 50
# 15 30 45
# s = (a1 + an) * n / 2 = (2*a1 + d*(n-1)) * n / 2

print(solution(6))
