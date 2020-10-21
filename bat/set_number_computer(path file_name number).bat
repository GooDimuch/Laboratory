@echo off
cd %1
Set infile=%2
Set find_str=number_computer
Set replace_ip=%3
for /f %%i in ('findstr /i %find_str% %infile%') do set find_line=%%i

setlocal enabledelayedexpansion
rename %infile% test.tmp
for /f "tokens=*" %%a in (test.tmp) do (
	set foo=%%a
	if "!foo!"=="%find_line%" (set foo=%find_str%,%3)
	echo !foo!>>%infile%
	)
del test.tmp
