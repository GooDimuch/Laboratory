#!/usr/bin/env python
# pylint: disable=C0116,W0613
# This program is dedicated to the public domain under the CC0 license.

"""
Basic example for a bot that works with polls. Only 3 people are allowed to interact with each
poll/quiz the bot generates. The preview command generates a closed poll/quiz, exactly like the
one the user sends the bot
"""
import os
import logging
import datetime, time
import configparser

from logging.handlers import TimedRotatingFileHandler
from pathlib import Path
from telegram import (
	Poll,
	Update,
)
from telegram.ext import (
	Updater,
	CommandHandler,
	JobQueue,
	CallbackContext,
)

Logger = None
POLL_TIME = 8
ACCESS_PARAMS_PATH = 'access_params.ini'
POLL_QUESTION = 'Подоляк тут?'
questions = ["Доброго ранку", "Ранку", "+"]
chat_id = -1


logging.basicConfig(
	format='%(asctime)s - %(name)s - %(levelname)s - %(message)s', level=logging.INFO
)
logger = logging.getLogger(__name__)

def init_logger():
	script_name = os.path.splitext(os.path.basename(__file__))[0]
	log_path = 'python_logs\\' + script_name + '.log'
	Path(os.path.dirname(log_path)).mkdir(parents=True, exist_ok=True)

	# format the log entries
	formatter = logging.Formatter('%(asctime)s %(levelname)s %(message)s')
	handler = TimedRotatingFileHandler(log_path, encoding='utf-8', when='midnight', backupCount=10)
	handler.setFormatter(formatter)

	logger = logging.getLogger(__name__)
	logger.addHandler(handler)
	logger.addHandler(logging.StreamHandler())
	logger.setLevel(logging.DEBUG)
	return logger

def close_logger(logger):
	handlers = logger.handlers[:]
	for handler in handlers:
		handler.close()
		logger.removeHandler(handler)
	logging.shutdown()
	return logger

def create_poll(context: CallbackContext) -> None:
	message = context.bot.send_poll(
		chat_id,
		POLL_QUESTION,
		questions,
		is_anonymous=False,
		allows_multiple_answers=False,
		disable_notification=True,
	)
	payload = {
		message.poll.id: {
			"questions": questions,
			"message_id": message.message_id,
			"chat_id": chat_id,
			"answers": 0,
		}
	}
	context.bot_data.update(payload)

def get_token() -> str:
	with open(ACCESS_PARAMS_PATH, 'r') as f:
		config_string = '[Params]\n' + f.read()
	config = configparser.ConfigParser()
	config.read_string(config_string)
	assert config['Params']['token']
	return config['Params']['token']

def daily_job(update: Update, context: CallbackContext):
	global chat_id
	t = datetime.time(POLL_TIME + int(time.timezone/3600), 00, 00, 000000)
	chat_id = update.effective_chat.id
	print(chat_id)
	context.job_queue.run_once(create_poll, 0, context=update)
	# context.job_queue.run_daily(create_poll, time=t, context=update)
	# context.job_queue.run_repeating(create_poll, 3, context=update)

def stop_poll(update: Update, context: CallbackContext):
	context.job_queue.stop()

def stop(message):
	if message.from_user.username == cfg.Father:	
		os.system(f'taskkill /pid {str(os.getpid())}')
	else:
		bot.send_message(message.chat.id, "Ты не Создатель бота; у тебя нет админ-прав, проваливай!")

def main() -> None:
	Logger = init_logger()
	updater = Updater(get_token())
	dispatcher = updater.dispatcher
	dispatcher.add_handler(CommandHandler('start', daily_job))
	dispatcher.add_handler(CommandHandler('stop', stop_poll))

	updater.start_polling()
	Logger.debug(f'start bot {(datetime.datetime.now())}')

	updater.idle()
	Logger.debug(f'stop bot {(datetime.datetime.now())}')
	close_logger(Logger)

if __name__ == '__main__':
	main()