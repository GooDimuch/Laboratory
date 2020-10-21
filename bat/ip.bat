@echo off
setlocal enabledelayedexpansion
for /f "usebackq tokens=*" %%a in (`ipconfig ^| findstr /i "ipv4"`) do (
  for /f delims^=^:^ tokens^=2 %%b in ('echo %%a') do (
    for /f "tokens=1-4 delims=." %%c in ("%%b") do (
      set _o1=%%c
      set _o2=%%d
      set _o3=%%e
      set _o4=%%f
      set _3octet=!_o1:~1!.!_o2!.!_o3!.!_o4!
      )
    )
  )
endlocal&set %1=%_3octet%
