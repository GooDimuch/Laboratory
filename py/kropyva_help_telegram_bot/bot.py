import telebot
import utils
from menu import Menu
from language import Language
from user import User

ACCESS_PARAMS_PATH = 'token.ini'
MENU_PATH = 'menu.json'
LANGUAGES_PATH = 'languages'
DEFAULT_LANGUAGE = 'ua'

bot = telebot.TeleBot(utils.Utils.get_token(ACCESS_PARAMS_PATH))
languages = utils.Utils.get_languages(LANGUAGES_PATH)
users = {}


@bot.message_handler(commands=['start'])
def start(message):
	user = get_user(message)
	user.menu.update_menu()
	user.current_item = user.menu
	bot.send_message(message.chat.id, user.current_item.text, reply_markup=user.current_item.markup)


def get_user(message) -> User:
	if message.chat.id in users:
		return users[message.chat.id]
	else:
		user = User(
			languages[DEFAULT_LANGUAGE],
			Menu(MENU_PATH, languages, current_language=languages[DEFAULT_LANGUAGE]))
		users[message.chat.id] = user
		return user


@bot.message_handler(content_types=['text'])
def user_message_handler(message):
	user = get_user(message)

	if user.current_item.is_last():
		user.current_item = user.current_item.parent.get_item_by_name(message.text)
	else:
		user.current_item = user.current_item.get_item_by_name(message.text)

	if user.current_item and \
			user.current_item.parent and \
			user.current_item.parent.name == user.current_language.translate(
		Menu.LANGUAGES_MENU_ITEM):
		change_menu_language(user)
		user.current_item = user.menu

	bot.send_message(message.chat.id, user.current_item.text, reply_markup=user.current_item.markup, parse_mode='html')


def change_menu_language(user: User):
	user.current_language = Language.find_language_by_name(languages, user.current_item.name, default=DEFAULT_LANGUAGE)
	user.menu.update_menu(user.current_language)


def main():
	print('Bot_started')
	bot.infinity_polling(skip_pending=True)
	print('Bot_stopped')
	pass


if __name__ == '__main__':
	main()
