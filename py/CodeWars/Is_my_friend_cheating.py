# sum + 1 = (a+1)(b+1)
def remov_nb(n):
	result = []
	if n < 2:
		return result
	sum_numbers = get_sum(n)

	for a in range(2, n):
		b = (sum_numbers + 1) / a
		if b % 1 == 0 and b - 1 <= n and a != b:
			result.append((a - 1, int(b - 1)))
	return result


def get_sum(n):
	return n * (n + 1) / 2


def print_debug():
	for n in range(100):
		result = remov_nb(n)
		if len(result) == 0:
			continue
		print(f'N = {n}\t{result}')

		for pair in result:
			print(f'{get_sum(n)} - {pair[0]} - {pair[1]} = {pair[0]} * {pair[1]}')


print_debug()
