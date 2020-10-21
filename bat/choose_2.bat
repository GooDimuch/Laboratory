@echo off

set res=F
choice /c yqwertuiopasdfghjklzxcvbnm1234567890 /m "%1 (yes(y), no(any key))?:"  /n
if not errorlevel 2 (
	set res=T
	)
