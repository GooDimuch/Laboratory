@echo off
hostname > __t.tmp
set /p hostname=<__t.tmp
del __t.tmp
set %1=%hostname%