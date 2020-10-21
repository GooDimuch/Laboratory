@echo off
set hierarchy_path=hierarchy.txt

if not exist %hierarchy_path% exit
if not exist empty exit

for /f %%i in (%hierarchy_path%) do (
	echo f | xcopy empty %%i /q
	)