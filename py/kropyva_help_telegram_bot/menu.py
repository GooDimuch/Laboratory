from __future__ import annotations

import json

from menu_item import MenuItem
from language import Language


class Menu(MenuItem):
	SETTINGS_MENU_ITEM = 'settings'
	LANGUAGES_MENU_ITEM = 'SELECT_LANGUAGE'

	languages = {}
	current_language = None

	file_path = ''

	def __init__(self, file_path: str, languages: dict[str], current_language: Language, text=''):
		self.languages = languages
		self.current_language = current_language
		self.file_path = file_path
		super(Menu, self).__init__('', self, self, text)
		self.update_menu()

	def __str__(self) -> str:
		sb = f'Menu [{self.current_language.short_name}]:\n' + \
		     '\t'.join(map(lambda item: str(item), self.items))
		return sb

	def update_menu(self, current_language=None):
		if current_language:
			self.current_language = current_language

		with open(self.file_path, 'r', encoding='utf-8') as f:
			main_item = MenuItem.from_json(json.loads(f.read()), self.current_language, self.root)
			self.__add_settings(main_item)

			self.items = main_item.items
			self.markup = main_item.markup

	def __add_settings(self, main_item):
		settings_item = self.__get_settings_item(main_item)
		main_item.add_items(settings_item)
		main_item.update_markup()

	def __get_settings_item(self, parent_item) -> MenuItem:
		settings_item = MenuItem(
			self.current_language.translate(Menu.SETTINGS_MENU_ITEM), self.root, parent_item)
		back_item = Menu.__get_back_menu_item(self.root, settings_item, self.current_language)
		languages_item = self.__get_languages_menu_item(settings_item)
		settings_item.add_items(back_item, languages_item, update_markup=True)
		return settings_item

	def __get_languages_menu_item(self, parent_item) -> MenuItem:
		languages_item = MenuItem(
			self.current_language.translate(Menu.LANGUAGES_MENU_ITEM), self.root, parent_item)
		for language in self.languages.values():
			language_item = MenuItem(language.name, self.root, languages_item)
			languages_item.add_items(language_item)
		languages_item.update_markup()
		return languages_item

	@staticmethod
	def __get_back_menu_item(root: Menu, parent: MenuItem, language: Language) -> MenuItem:
		return MenuItem(language.translate(MenuItem.BACK_MENU_ITEM), root, parent)
