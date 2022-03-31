import xml.etree.ElementTree as ET


class Language:
	name = ''
	short_name = ''
	constants = {}
	item_names = {}

	def __init__(self, file_path):
		root = ET.parse(file_path).getroot()
		self.name = root.attrib['name']
		self.short_name = root.attrib['short_name']
		self.constants = {}
		self.item_names = {}

		for child in root.find('constants'):
			self.constants[child.attrib['key']] = child.text

		for child in root.find('item_names'):
			self.item_names[child.attrib['key']] = child.text

	def translate(self, key):
		if key in self.constants:
			return self.constants[key]
		if key in self.item_names:
			return self.item_names[key]
		return key

	@staticmethod
	def find_language_by_name(languages, name, default='ua'):
		for language in languages.values():
			if language.name == name:
				return language
		return languages[default]

	def __str__(self) -> str:
		return f'{type(self)} (Name: {self.name}[{self.short_name}])' \
			# f'\nConstants: [{", ".join(map(lambda item: str(item), self.constants))}]' \
	# f'\nItem_names: [{", ".join(map(lambda item: str(item), self.item_names))}]'
