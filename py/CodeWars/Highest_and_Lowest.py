def high_and_low(numbers):
	int_numbers = list(map(lambda s: int(s), numbers.split(' ')))
	return f'{max(int_numbers)} {min(int_numbers)}'


numbers = "8 3 -5 42 -1 0 0 -9 4 7 4 -4"
print(high_and_low(numbers))
