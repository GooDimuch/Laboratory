@echo off

set vbs=HKLM\Software\BISim\VBS\
reg query %vbs% || goto :error

for /F "tokens=1" %%a in ('reg query %vbs%') DO (
	set lastVersion=%%a
	)
reg query %lastVersion% || goto :error
echo last = %lastVersion%
pause

for /F "tokens=1" %%a in ('REG QUERY %lastVersion%') DO (
	set firstDir=%%a
	echo %%a
	goto :break
	)
:break
reg query %firstDir% || goto :error
echo firstDir = %firstDir%
pause

for /f "tokens=3*" %%a in ('reg query %firstDir% /V InstallDir ^|findstr /ri "REG_SZ"') do (
	set path=%%a %%b
	)
	echo %path%
	pause
if exist "%path%" (goto :return) else (goto :error)

:error
echo ERROR
set /p path=Enter path to VBS: 
if exist "%path%" (goto :return) else (goto :error)
goto :return

:return
set %1=%path%