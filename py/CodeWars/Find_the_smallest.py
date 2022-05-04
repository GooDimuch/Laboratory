import codewars_test as test


def smallest(n):
	digits = number_to_digits(n)
	best_result = sorted(digits.copy())
	to_index, value = get_to_index(digits, best_result)
	from_index = find_right_index(digits, value)
	insert_digit(digits, from_index, to_index)

	return [digits_to_number(digits), from_index, to_index] \
		if from_index - to_index != 1 \
		else [digits_to_number(digits), to_index, from_index]


def number_to_digits(num):
	return [int(a) for a in str(num)]


def digits_to_number(digits):
	return int(''.join(map(str, digits)))


def get_to_index(digits, best_result):
	for i, digit in enumerate(digits):
		if best_result[i] < digit:
			return get_min_index(digits, i-1), best_result[i]
	return 0, best_result[0]


def find_right_index(digits, value):
	reversed_digits = digits.copy()
	reversed_digits.reverse()
	for i, digit in enumerate(reversed_digits):
		if digit == value and i + 1 < len(reversed_digits) and reversed_digits[i + 1] != value:
			return get_min_index(digits, len(digits) - 1 - i)
	return -1


def get_min_index(digits, index):
	print(digits, index)
	for i in range(index, -1, -1):
		print(i, digits[index], digits[i])
		if digits[index] != digits[i]:
			print(f'min {i+1}')
			return i + 1
	print(f'normal {0}')
	return 0


def insert_digit(digits, _from, to):
	digits.insert(to, digits[_from])
	digits.pop(_from + 1)


def testing(n, res):
	print(n)
	test.assert_equals(smallest(n), res)


test.describe("smallest")
test.it("Basic tests")
testing(261235, [126235, 2, 0])
testing(209917, [29917, 0, 1])
testing(285365, [238565, 3, 1])
testing(269045, [26945, 3, 0])
testing(296837, [239687, 4, 1])
testing(199819884756, [119989884756, 4, 0])
