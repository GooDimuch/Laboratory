def loop_size(node):
	hash_list = []
	while node.next:
		hash_code = node.value
		# hash_code = hash(node)
		if hash_code in hash_list:
			return len(hash_list) - hash_list.index(hash_code)
		hash_list.append(hash_code)
		node = node.next
	return 0


class Node:
	value = None
	next = None

	def __init__(self, value):
		self.value = value

	def add(self, value):
		if self.next:
			self.next.add(value)
		else:
			self.next = Node(value)


def init_list():
	linked_list = Node(-3)
	linked_list.add(-2)
	linked_list.add(-1)
	linked_list.add(0)
	linked_list.add(1)
	linked_list.add(2)
	linked_list.add(3)
	linked_list.add(4)
	linked_list.add(5)
	linked_list.add(6)
	linked_list.add(7)
	linked_list.add(8)
	linked_list.add(9)
	linked_list.add(10)
	linked_list.add(11)
	linked_list.add(12)
	linked_list.add(1)
	linked_list.add(2)
	linked_list.add(3)
	linked_list.add(4)

	return linked_list


def print_linked_list(linked_list):
	result = ''
	node = linked_list
	while node.next:
		result += f'{node.test} -> '
		node = node.next
	result += f'{node.test}'
	print(result)


list = init_list()
print_linked_list(list)
print(loop_size(list))
