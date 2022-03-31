# python -m pip install sounddevice
# python -m pip install soundfile
# python -m pip install numpy

def import_or_install(package):
	try:
		return __import__(package)
	except ImportError:
		pip.main(['install', package])

import pip
import os
import sys
import time
import threading
sd = import_or_install('sounddevice')
sf = import_or_install('soundfile')
keyboard = import_or_install('keyboard')

device_id = 0
devices = len(sd.query_devices())
data, fs = None, None
sample_thread = None
last_dir = 'n'
last_worked_device = 0
exit = False

def next_device(name):
	global device_id, last_dir
	device_id = last_worked_device if (device_id + 1 > devices - 1) else device_id + 1
	last_dir = name
	updateUI()

def previos_device(name):
	global device_id, last_dir
	device_id = last_worked_device if (device_id - 1 < 0) else device_id - 1
	last_dir = name
	updateUI()

def updateUI_Pressed(name):
	updateUI()

def main(sample_path):
	global exit, data, fs, sample_thread

	data, fs = sf.read(sample_path)

	keyboard.on_press_key('n', next_device)
	keyboard.on_press_key('p', previos_device)
	keyboard.on_press_key('u', updateUI_Pressed)

	sample_thread = threading.Thread(target=play_sample)
	sample_thread.start()

	updateUI()

	while not keyboard.is_pressed('esc'):
		keyboard.read_key()
	exit = True

def updateUI():
	os.system('cls')
	print("Press\n" + "\t'esc' for exit\n" + "\t'n' for next device\n" + "\t'p' for previos device")
	print()
	print(f'Current device id: {device_id}')

def play_sample():
	while not exit:
		play_sample_once(data, fs, device_id)

def play_sample_once(data, fs, device_id):
	global last_worked_device
	try:
		sd.default.device = device_id
		sd.play(data, fs)
		sd.wait()
		last_worked_device = device_id
	except (sd.PortAudioError):
		if (last_dir == 'n'):
			next_device(last_dir)
		else:
			previos_device(last_dir)

if __name__ == "__main__":
	if (len(sys.argv) > 2):
		main(sys.argv[1])
	else:
		main('test.wav')
		# play_sample_once('test.wav', 3)
		# print('ArgumentException')

