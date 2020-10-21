NumpadIns::
if WinActive("ahk_exe LMS_Preview.exe") {
	WinActivate, ahk_exe VBS3_64.exe
	MouseMove, 1920/2, 1080/2
	;MsgBox LMS_Preview.
	return
}

if WinActive("ahk_exe builds.exe") {
	WinActivate, ahk_exe LMS_Preview.exe
	MouseMove, 1920/2, 1080/2 
	;MsgBox build.
	return
}

if WinActive("ahk_exe VBS3_64.exe") {
	WinActivate, ahk_exe builds.exe
	MouseMove, 1920/2, 1080/2    
	;MsgBox VBS3_64.
	return
}
return