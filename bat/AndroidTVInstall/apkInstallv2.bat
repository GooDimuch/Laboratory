@echo off
chcp 1253
SET defaultLocation=%cd%\ApkToInstall
SETLOCAL EnableDelayedExpansion
for /F "tokens=1,2 delims=#" %%a in ('"prompt #$H#$E# & echo on & for %%b in (1) do rem"') do (
  set "DEL=%%a"
)
cls
tasklist /nh /fi "imagename eq adb.exe" | find /i "adb.exe" > nul || adb start-server
echo -------------------------------------------------
call :ColorText 03 "                    mBEDed"
echo.
for /f %%a in ('"prompt $H&for %%b in (1) do rem"') do set "bs=%%a"
echo|set /p var=.%bs%                            µ
call :ColorText 03 "Systems"  
echo.
set /p IP= Enter ip:
adb connect %IP% | find /i "connected to" >nul
if errorlevel 1 (
    echo Not successful
) else (
    echo Successful
)
echo -------------------------------------------------
echo ADB is waiting for the device. Please make sure you have
echo 1. Connected your device to the system.
echo 2. Enabled Android Debugging in Developer Options.
echo -------------------------------------------------
adb wait-for-device
echo -------------------------------------------------
adb devices
echo. 
echo Apps will install from "%defaultLocation%" folder.
echo PS :- You can change "default loaction" in the 2nd line of the batch file.
echo.
set "opt=x"
set /p opt=Are you ready to install ? [(Y)es/(N)o/(C)hange]:
:correctOption
if %opt%==y goto installApps
if %opt%==Y goto installApps
if %opt%==c goto changeLocation
if %opt%==C goto changeLocation
if %opt%==n goto processHalt
if %opt%==N goto processHalt
echo.
echo.
echo Wrong input!   Correct Options :- Y=Yes    N=No    C=Change
set /p opt=Please choose one from above:
goto correctOption
:changeLocation
echo.
echo.
echo Please enter new location.
set /p defaultLocation=^>
:installApps
if not EXIST "%defaultLocation%\*.apk" ( 
echo.
echo The folder you entered dosen't contains any apk files.
echo Please try again...
goto changeLocation
)
echo Installing apps to phone...
echo.
for /f "delims=|" %%i in ('dir /b "%defaultLocation%\*.apk"') do (
	echo Installing %%~ni
	adb install -r "%defaultLocation%\%%i"
)
echo.
echo.
echo Congratulations! The installation process has been completed.
goto wannaMore
:processHalt
echo.
echo.
echo Process Stopped.
:wannaMore
set /p opt=Are you want to install app one more time? [(Y)es/(N)o]:
:wannaMoreOption
if %opt%==y goto installApps
if %opt%==Y goto installApps
if %opt%==n goto processComplete
if %opt%==N goto processComplete
echo.
:processComplete
adb kill-server
echo.
pause
exit
:ColorText
echo off
<nul set /p ".=%DEL%" > "%~2"
findstr /v /a:%1 /R "^$" "%~2" nul
del "%~2" > nul 2>&1
goto :eof