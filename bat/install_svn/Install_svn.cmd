echo off
msiexec /i "TortoiseSVN.msi" /passive INSTALLDIR="C:\Program Files\TortoiseSVN" ADDLOCAL=ALL
svn
pause
start /i "%windir%\explorer.exe" continue_Install