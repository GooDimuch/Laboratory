import re


# return masked string
def maskify(cc):
	print(re.sub('.(?!.{0,3}$)', '#', cc))


maskify('1s234567890')