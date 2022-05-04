from functools import cmp_to_key


def order_weight(string: str):
	if not string: return ''
	pairs = list(map(lambda s: (int(s), get_sum_digits(int(s))), string.split(' ')))
	pairs = sorted(pairs, key=cmp_to_key(compare))
	return ' '.join(map(lambda pair: str(pair[0]), pairs))


def get_sum_digits(n):
	sum = 0
	while n > 0:
		sum += int(n % 10)
		n = n // 10
	return sum


def compare(item1, item2):
	if item1[1] < item2[1]:
		return -1
	elif item1[1] > item2[1]:
		return 1
	else:
		return 1 if str(item1[0]) > str(item2[0]) else -1


print(order_weight("103 123 4444 99 2000 60"))
print(order_weight(""))
