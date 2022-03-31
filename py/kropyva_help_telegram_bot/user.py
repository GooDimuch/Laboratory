from menu import Menu
from menu import MenuItem
from language import Language


class User:
	current_language = None
	menu = None
	current_item = None

	def __init__(self, default_language, menu):
		self.current_language = default_language
		self.menu = menu
		self.current_item = menu
