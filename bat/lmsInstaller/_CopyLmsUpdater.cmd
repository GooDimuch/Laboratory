if exist c:\LmsUpdater (
rmdir c:\LmsUpdater /S /Q
)
md c:\LmsUpdater
echo Copying LmsUpdater to c:\LmsUpdater folder
xcopy "%subjectsDir%LMSUpdater" "c:\LMSUpdater" /E /Y /Q
