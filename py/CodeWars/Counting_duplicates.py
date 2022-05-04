char_list = []


def duplicate_count(string: str):
	char_list.clear()

	for char in string.lower():
		print(char)
		if string.lower().count(char) > 1:
			char_list.append(char)

	return len(dict.fromkeys(char_list))


print(duplicate_count("abcdeaB"))
