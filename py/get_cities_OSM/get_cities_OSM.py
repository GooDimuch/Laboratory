# pip install translitua
# pip install OSMPythonTools

def import_or_install(package):
	try:
		return __import__(package)
	except ImportError:
		pip.main(['install', package])

import pip
import sys, os
import ssl
import csv, sqlite3
from pathlib import Path
translitua = import_or_install('translitua')
OSMPythonTools = import_or_install('OSMPythonTools')
import translitua as tr
from OSMPythonTools.nominatim import Nominatim
from OSMPythonTools.overpass import overpassQueryBuilder, Overpass

try:
	_create_unverified_https_context = ssl._create_unverified_context
except AttributeError:
	pass
else:
	ssl._create_default_https_context = _create_unverified_https_context

class City:
	def __init__(self, id, city_key, name, old_name, district, region, x, y):
		self.id = id
		self.city_key = city_key
		self.override(name, old_name, district, region, x, y)

	def override(self, name, old_name, district, region, x = None, y = None):
		self.name = name or ''
		self.old_name = old_name or ''
		self.district = district or ''
		self.region = region or ''
		self.x = x or self.x
		self.y = y or self.y

	def get_key(self):
		return f'{self.id}, {self.name}, {self.district}, {self.region}'.lower()
		
	def __str__(self):
			return f'{self.id}, {self.name}, {self.district}, {self.region}, {self.x}, {self.y}'

CONFLICT_DISTRICT = ''
CITIES_CSV_PATH = 'cities.csv'
DB_TABLE_NAME = 'easternCity'
CITIES_DB_PATH = DB_TABLE_NAME + '.sqlite'
OVERRIDE_CITIES_CSV_PATH = 'override_cities.csv'
CONFLICT_CITIES_CSV_PATH = 'conflict_cities.csv'
TIMEOUT_REQUEST = 300
PRINT_ALL = False
PRINT_REGIONS = False
PRINT_DISTRICTS = False
PRINT_CITIES = False
# PRINT_REGIONS = True
# PRINT_DISTRICTS = True
# PRINT_CITIES = True
conflict_cities = {}

def sum_city_keys(cities_counter, city_keys):
	for key in cities_counter.keys():
		cities_counter[key] += city_keys[key]

def cities_counter_to_string(cities_counter):
	result = ', '.join([f'{key}[{value}]' for key, value in cities_counter.items()])
	return f'{sum(cities_counter.values())} ({result})'

def append_cities_by_district(cities, overpass, district_osm, region_name, district_name, localization = 'en'):
	city_keys = {'city': 0, 'town': 0, 'village': 0, 'hamlet': 0, 'neighbourhood': 0}
	for city_key in city_keys.keys():
		query = overpassQueryBuilder(area=district_osm.areaId(), elementType='node', selector=[f'"place"="{city_key}"'], out='body')
		result = overpass.query(query, timeout=TIMEOUT_REQUEST)
		# print(result.toJSON())

		for city_osm in result.elements():
			city_name = city_osm.tag(f'name:{localization}') if (city_osm.tag(f'name:{localization}')) else city_osm.tag('name')
			city_old_name = city_osm.tag(f'old_name:{localization}') if (city_osm.tag(f'old_name:{localization}')) else city_osm.tag('old_name')
			# print(f'\t\t{city_name}')
			city = City(city_osm.id(), city_key, city_name, city_old_name, district_name, region_name, city_osm.lon(), city_osm.lat())
			if (city.get_key() in cities):
				# print(f'Error. City already exist [{city}]')
				# cities[city.get_key()].district += CONFLICT_DISTRICT
				# city.district += CONFLICT_DISTRICT + str(city.id)
				conflict_cities[city.get_key()] = city
				pass
				# continue
			cities[city.get_key()] = city
			city_keys[city_key] += 1
			if (PRINT_ALL or PRINT_CITIES): print(city)
		# if (city_keys[city_key] > 0): print(f'{city_keys[city_key]} {city_key} added')
	return city_keys

def append_cities_by_region(cities, overpass, region_osm, region_name, localization = 'en'):
	if (PRINT_ALL or PRINT_REGIONS): print(f'{region_name}')
	cities_counter = None

	query = overpassQueryBuilder(area=region_osm.areaId(), elementType='nwr', selector=['"place"="district"'], out='body')
	result = overpass.query(query, timeout=TIMEOUT_REQUEST)
	# print(result.toJSON())

	districts = result.elements()
	if (len(districts) > 0):
		for district_osm in result.elements():
			district_name = district_osm.tag(f'name:{localization}') if (district_osm.tag(f'name:{localization}')) else district_osm.tag('name')
			if (PRINT_ALL or PRINT_DISTRICTS): print(f'\t{district_name}')
			city_keys = append_cities_by_district(cities, overpass, district_osm, region_name, district_name, localization)
			if (cities_counter): sum_city_keys(cities_counter, city_keys)
			else: cities_counter = city_keys
	else:
		cities_counter = append_cities_by_district(cities, overpass, region_osm, region_name, '', localization)
	return cities_counter

def append_cities_by_contry(cities, nominatim, overpass, contry, skip_regions = {}, localization = 'en'):
	cities_counter = None

	areaId = nominatim.query(contry).areaId()
	query = overpassQueryBuilder(area=areaId, elementType='relation', selector=['"admin_level"="4"'], out='body')
	result = overpass.query(query, timeout=TIMEOUT_REQUEST)
	# print(result.toJSON())

	for region_osm in result.elements():
		region_name = region_osm.tag(f'name:{localization}') if (region_osm.tag(f'name:{localization}')) else region_osm.tag('name')
		if (region_name in skip_regions): continue
		if (region_osm.id() in skip_regions): continue
		city_keys = append_cities_by_region(cities, overpass, region_osm, region_name, localization)
		if (cities_counter): sum_city_keys(cities_counter, city_keys)
		else: cities_counter = city_keys

	return cities_counter

def append_ukrainian_cities(cities, nominatim, overpass):
	return append_cities_by_contry(cities, nominatim, overpass, 'Ukraine', {3788485, 3795586}, 'uk')

def append_cities_by_regions(cities, nominatim, overpass, regions):
	cities_counter = None

	for region_name in regions:
		region_osm = nominatim.query(region_name)
		city_keys = append_cities_by_region(cities, overpass, region_osm, region_name)
		if (cities_counter): sum_city_keys(cities_counter, city_keys)
		else: cities_counter = city_keys
	return cities_counter

def append_russian_cities(cities, nominatim, overpass):
	regions = {
		'Красноярский край',
		'Ростовская область',
		'Воронежская область',
		'Белгородская область',
		'Курская область',
		'Брянская область',
	}
	return append_cities_by_regions(cities, nominatim, overpass, regions)

def append_belarusian_cities(cities, nominatim, overpass):
	regions = {
		'Брестская область',
		'Гомельская область',
	}
	return append_cities_by_regions(cities, nominatim, overpass, regions)

def append_moldova_cities(cities, nominatim, overpass):
	regions = {
		'Окницький район',
		'Дондушенський район',
		'Сороцький район',
		'Адміністративно-територіальні одиниці лівобережжя Дністра',
	}
	return append_cities_by_regions(cities, nominatim, overpass, regions)

def append_neighbor_cities(cities, nominatim, overpass):
	russian_cities_counter = append_russian_cities(cities, nominatim, overpass)
	print(f'Russian cities: {cities_counter_to_string(russian_cities_counter)}')
	belarusian_cities_counter = append_belarusian_cities(cities, nominatim, overpass)
	print(f'Belarusian cities: {cities_counter_to_string(belarusian_cities_counter)}')
	moldova_cities_counter = append_moldova_cities(cities, nominatim, overpass)
	print(f'Moldova cities: {cities_counter_to_string(moldova_cities_counter)}')

def get_cities():
	city_dictionary = {}

	nominatim = Nominatim()
	overpass = Overpass()

	ukrainian_cities_counter = append_ukrainian_cities(city_dictionary, nominatim, overpass)
	print(f'Ukrainian cities: {cities_counter_to_string(ukrainian_cities_counter)}')
	append_neighbor_cities(city_dictionary, nominatim, overpass)
	print(f'All cities: {len(city_dictionary.keys())}')
	print(f'Conflicted: {len(conflict_cities.keys())}')

	return city_dictionary

def override(cities):
	if (Path(OVERRIDE_CITIES_CSV_PATH).is_file()):
		with open(OVERRIDE_CITIES_CSV_PATH, mode='r', encoding='UTF8', newline='') as csv_file:
			csv_reader = csv.DictReader(csv_file)
			override_city_counter = 0
			for row in csv_reader:
				city_id = int(row['id'])
				city = next((city for city in cities.values() if city.id == city_id), None)
				if (city):
					if (row['x'] and row['y']):
						city.override(row['name'], row['old_name'], row['district'], row['region'], row['x'], row['y'])
					else:
						cities.pop(city.get_key(), None)
				else:
					city = City(row['id'], row['key'] or "city", row['name'], row['old_name'], row['district'], row['region'], row['x'], row['y'])
					cities[city.get_key()] = city
				override_city_counter += 1
			print(f'Override {override_city_counter} cities')

def translit(cities):
	for city in cities.values():
		city.override(
			tr.translit(city.name, tr.RussianSimple),
			tr.translit(city.old_name, tr.RussianSimple),
			tr.translit(city.district, tr.RussianSimple),
			tr.translit(city.region, tr.RussianSimple))

def write(cities, file_path):
	with open(file_path, mode='w', encoding='UTF8', newline='') as csv_file:
		fieldnames = ['id', 'old_name', 'name', 'district', 'region', 'x', 'y']
		writer = csv.DictWriter(csv_file, fieldnames=fieldnames)

		writer.writeheader()

		for key, city in cities.items():
			writer.writerow({
				'id': city.id, 
				'old_name': city.old_name, 
				'name': city.name, 
				'district': city.district, 
				'region': city.region, 
				'x': city.x, 
				'y': city.y})

def create_db(csv_file_path, db_file_path):
	if (Path(db_file_path).is_file()):
		os.remove(db_file_path)
	con = sqlite3.connect(db_file_path)
	cur = con.cursor()
	cur.execute('CREATE TABLE ' + DB_TABLE_NAME + ' (id, old_name, name, district, region, x, y);')

	with open(csv_file_path, mode='r', encoding='UTF8', newline='') as csv_file:
		reader = csv.DictReader(csv_file)
		to_db = [(row['id'], row['old_name'], row['name'], row['district'], row['region'], row['x'], row['y']) for row in reader]

	cur.executemany('INSERT INTO ' + DB_TABLE_NAME + ' (id, old_name, name, district, region, x, y) VALUES (?, ?, ?, ?, ?, ?, ?);', to_db)
	con.commit()
	con.close()
	pass

def main() -> int:
	cities = get_cities()
	override(cities)
	# translit(cities)
	write(cities, CITIES_CSV_PATH)
	create_db(CITIES_CSV_PATH, CITIES_DB_PATH)

	# write(conflict_cities, CONFLICT_CITIES_CSV_PATH)
	return 0

if __name__ == '__main__':
	main()
