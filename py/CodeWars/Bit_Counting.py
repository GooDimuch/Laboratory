def count_bits(n):
	v = 1 << 0
	flag = 0
	count = 0
	while v <= n:
		if ((1 << flag) & n) == 1 << flag:
			count += 1
		flag += 1
		v = 1 << flag
	return count


print(count_bits(4))
