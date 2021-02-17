set dirName=%1
set installServerIPAdress=%2

if not exist c:\lms (
md c:\LMS
)

cd /d c:\lms

if exist "%cd%\%dirName%" (
rmdir "%dirName%" /S /Q
)
echo Ready to fetch %dirName% from server on %installServerIPAdress%
if "%silentMode%"=="v" (
pause
)
svn checkout svn://%installServerIPAdress%/lms/LMS/%dirName% "%cd%\%dirName%"

if "%silentMode%"=="v" (
pause
)

echo Copying files to _settings folder
md "%cd%\%dirName%\Lms%dirName%\_settings"
xcopy "%~dp0..\Settings\%dirName%" "%cd%\%dirName%\Lms%dirName%\_settings" /E /Y /Q

echo Creating desktop shortcut
cd /d %UserProfile%\Desktop
mklink LMS_%dirName% "c:\lms\%dirName%\Lms%dirName%\lms%dirName%.exe"