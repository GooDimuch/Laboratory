import itertools
# import sympy
# from sympy import divisors, divisor_count

n = 10

def remov_nb(n):
	sumNumbers = get_sum(n)
	pairs = get_pairs(n)
	result = findPairs(pairs, sumNumbers)
	appendPermutations(result)
	result.sort()
	return result

def get_sum(n):
	return n * (n + 1) / 2

def get_pairs(n):
	return list(itertools.combinations([i + 1 for i in range(n)], 2))

def findPairs(pairs, sumNumbers):
	result = []
	for pair in pairs:
		if (numbers_is_suitable(pair, sumNumbers)):
			result.append(pair)
	return result

def numbers_is_suitable(pair, sumNumbers):
	return pair[0] * pair[1] == sumNumbers - pair[0] - pair[1]

def appendPermutations(pairs):
	for i in range(len(pairs)):
		pairs.append((pairs[i][1], pairs[i][0]))

print(remov_nb(n))
print(str(int(get_sum(n))) + " - " + str(remov_nb(n)[0][0]) + " - " + str(remov_nb(n)[0][1]) + " = " + str(remov_nb(n)[0][0] * remov_nb(n)[0][1]))

# sympy.ntheory.factor_.divisors