@echo off
set /p x=Install VisualSVN (yes(y), no(any key))?: 
set res=F
if /i %x%==y set res=T
if %res%==T echo +
pause