import os
import psutil
import git
import ctypes
import subprocess

import time
import logging
from logging.handlers import TimedRotatingFileHandler
from pathlib import Path

MessageBox = ctypes.windll.user32.MessageBoxW
Logger = None

def __main__():
	print("__main__")
	pass

def __init__():
	print("__init__")
	pass

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

def close_process(**name_or_id):
	Logger.debug('close_process ' + name_or_id.get('name'))
	for process in psutil.process_iter():
		if (name_or_id.get('id') is not None and process.pid == name_or_id['id'] or
				name_or_id.get('name') is not None and os.path.splitext(process.name())[0] == os.path.splitext(name_or_id['name'])[0]):
			Logger.info('Kill process [' + os.path.splitext(process.name())[0] + ']')
			process.kill()
			return

def update(repo):
	Logger.debug('update')
	try:
		pullInfos = repo.git.pull()
		Logger.debug(pullInfos)
	except Exception as err:
		show_message(str(err))

def try_update(repo_path):
	Logger.debug('try_update')
	repo = git.Repo(repo_path)
	if (hasUpdate(repo)):
		update(repo)
	else:
		show_message('has not update')

def hasUpdate(repo_path):
	Logger.debug('hasUpdate')
	repo = git.Repo(repo_path) if (isinstance(repo_path, str)) else repo_path
	pullInfos = repo.remotes.origin.fetch()
	commits_behind = repo.iter_commits('master..origin/master')
	count_behind = sum(1 for c in commits_behind)
	return count_behind > 0

def show_message(message):
	MessageBox(None, message, 'Puller', 0x00000040)
	Logger.error(message)

def start_update(app_path, repo_path):
	global Logger
	Logger = init_logger()
	close_process(name = os.path.basename(os.path.splitext(app_path)[0]))
	try_update(repo_path)
	subprocess.run(app_path)
	close_logger(Logger)

def check_update(repo_path):
	global Logger
	Logger = init_logger()
	result = hasUpdate(repo_path)
	close_logger(Logger)
	return result

# -c "from puller import check_update; print(check_update('PATH'))"