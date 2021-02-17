echo Creating temp checkout folder named %CheckoutFolderName%
cd /d c:\
if exist "C:\%CheckoutFolderName%\" (
rmdir "C:\%CheckoutFolderName%" /S /Q
)
md "C:\%CheckoutFolderName%"
cd /d "C:\%CheckoutFolderName%"
svn checkout svn://127.0.0.1/lms "%cd%" %quietFlag%

if "%silentMode%"=="v" (
pause
)