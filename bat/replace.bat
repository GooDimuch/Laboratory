@echo off

set infile=wc.db

for /f "delims=" %%i in (%infile%) do (
	set line=%%i
	echo %line%
	)
pause