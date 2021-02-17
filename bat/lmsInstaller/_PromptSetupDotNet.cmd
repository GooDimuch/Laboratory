set res=Null
call "%~dp0choose_2" Do_you_want_to_install_MS_.NET_Framework_Developer_pack
if "%res%"=="T" (
echo Please wait while dotNET is setting up
call "%packagesDir%ndp48-devpack-enu.exe" /q
)