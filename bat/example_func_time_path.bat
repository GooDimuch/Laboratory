@echo off
set LOG_PATH=%~dp0temp.log
set LOGGER_PATH=%~dp0logger.bat

call :getLocalDateTime LDT LD
call :getFileInfo %LOG_PATH% LOG_ROOT LOG_NAME LOG_EXTENSION

set NEW_LOG_NAME=%LOG_NAME%_%LDT%%LOG_EXTENSION%

if not exist %LOG_ROOT%%LD% if exist %LOG_PATH% (
  md %LD%
  rename %LOG_PATH% %NEW_LOG_NAME%
  move %LOG_ROOT%%NEW_LOG_NAME% %LOG_ROOT%%LD%
)
call %LOGGER_PATH%
rem pause


rem -----------------------------------------------------
:getLocalDateTime
  setlocal

  for /F "usebackq tokens=1,2 delims==" %%i in (`wmic os get LocalDateTime /VALUE 2^>NUL`) do if '.%%i.'=='.LocalDateTime.' set ldt=%%j
  set ldt=%ldt:~6,2%-%ldt:~4,2%-%ldt:~0,4%_%ldt:~8,2%-%ldt:~10,2%-%ldt:~12,2%

  ( endlocal
    set "%1=%ldt%"
    set "%2=%ldt:~0,10%"
  )
exit /b
rem -----------------------------------------------------

rem -----------------------------------------------------
:getFileInfo
  setlocal

  for %%F in (%1) do (
    set "NAME=%%~nF"
    set "EXTENSION=%%~xF"
  )
  for %%a in (%1) do set "ROOT=%%~dpa"

  ( endlocal
    set "%2=%ROOT%"
    set "%3=%NAME%"
    set "%4=%EXTENSION%"
  )
exit /b
rem -----------------------------------------------------

