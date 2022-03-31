from __future__ import annotations

import telebot
import json

from typing import Any

from language import Language


class MenuItem:
	SELECT_MENU_ITEM = 'SELECT_MENU_ITEM'
	BACK_MENU_ITEM = 'BACK'
	MARKUP_RESIZE_KEYBOARD = True
	MARKUP_ROW_WIDTH = 1

	text = ''
	name = ''
	items = None
	markup = None
	root = None
	parent = None

	def __init__(self, name, root, parent: MenuItem = None, text=''):
		self.text = text or root.current_language.translate(MenuItem.SELECT_MENU_ITEM)
		self.name = name
		self.root = root
		self.parent = parent
		self.items = []
		self.markup = None

	def __str__(self) -> str:
		return f'\t{self.name}\n' + '\t\t'.join(map(lambda item: str(item), self.items))

	def is_last(self) -> bool:
		return not self.items

	def add_items(self, *items, update_markup=False):
		self.items.extend(items)
		if update_markup:
			self.update_markup()

	def get_item_by_name(self, name) -> MenuItem:
		if name == self.root.current_language.translate(MenuItem.BACK_MENU_ITEM):
			return self.parent or self.root
		for item in self.items:
			if item.name == name:
				return item
		return self.root

	def update_markup(self):
		# if len(self.items) == 0:
		# 	self.markup = telebot.types.ReplyKeyboardRemove()
		# else:
		self.markup = telebot.types.ReplyKeyboardMarkup(
			resize_keyboard=MenuItem.MARKUP_RESIZE_KEYBOARD,
			row_width=MenuItem.MARKUP_ROW_WIDTH)
		for button in map(lambda item: telebot.types.KeyboardButton(item.name), self.items):
			self.markup.add(button)

	def to_dict(self) -> Any:
		return {
			'text': self.text,
			'name': self.name,
			'items': self.items,
		}

	def to_json(self) -> str:
		return json.dumps(self, default=lambda o: o.to_dict(), indent=4)

	@staticmethod
	def from_json(json_obj, language: Language, root) -> MenuItem:
		menu_item = MenuItem(language.translate(json_obj['name']), root, text=language.translate(json_obj['text']))
		for json_item in json_obj['items']:
			item = MenuItem.from_json(json_item, language, root)
			item.parent = menu_item
			menu_item.add_items(item)
		menu_item.update_markup()
		return menu_item
