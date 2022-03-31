import os
import configparser
from os import listdir
from os.path import isfile, join
from language import Language


class Utils:
	@staticmethod
	def get_token(file_path) -> str:
		with open(file_path, 'r') as f:
			config_string = '[Params]\n' + f.read()
		config = configparser.ConfigParser()
		config.read_string(config_string)
		assert config['Params']['token']
		return config['Params']['token']

	@staticmethod
	def get_languages(directory_path) -> dict[str]:
		lang_file_names = [f for f in listdir(directory_path) if isfile(join(directory_path, f))]
		languages = {}
		for lang_file_name in lang_file_names:
			language = Language(os.path.join(directory_path, lang_file_name))
			languages[language.short_name] = language
		return languages
