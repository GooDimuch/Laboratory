set res=Null
call "%~dp0choose_2" Do_you_want_to_install_TortoiseSVN
if "%res%"=="T" (
call msiexec /i "%packagesDir%TortoiseSVN.msi" /passive /le "%~dp0install_TortoiseSVN.log" INSTALLDIR="%svnDestinationDir%" ADDLOCAL=ALL
call "%~dp0_setupSvn"
)