PgUp::
;WinGetPos,,, Width, Height,A
;MsgBox, Width=%Width% Height=%Height%
SysGet, Mon2, Monitor, 1
;MsgBox, Left: %Mon2Left% -- Top: %Mon2Top% -- Right: %Mon2Right% -- Bottom %Mon2Bottom%.

WinActivate, ahk_exe builds.exe
if((global MyNumber) == 1){
	;MsgBox % "MyNumber = ".global MyNumber
	global MyNumber = 0
	WinMove,ahk_exe builds.exe,,0, Mon2Bottom
}else{
	;MsgBox % "MyNumber != ".global MyNumber
	global MyNumber = 1
	WinMove,ahk_exe builds.exe,,0, 0
}
return