global isSwapped = false
global isGunner = true


Loop 
{
	if !(isSwapped)
	{
		SwapWindow()
	} 
	else 
	{
		CheckCloseWindow()
	}
	Sleep, 1000
}
return

PgUp::
global isGunner = true
ActivateWindow()
return

PgDn:: 
global isGunner = false
ActivateWindow()
return

ActivateWindow()
{
	if (global isGunner) 
	{
		IfWinExist, ahk_exe vbs3_64.exe
		{
			WinActivate, ahk_exe vbs3_64.exe
			return true
			MsgBox ActivateWindow
		}
	} 
	else 
	{
		IfWinExist, ahk_exe vbs3_64_ptur.exe
		{
			WinActivate, ahk_exe vbs3_64_ptur.exe
			return true
			MsgBox ActivateWindow
		}
	}
	return false
}

SwapWindow()
{
	IfWinExist, ahk_exe vbs3_64.exe
	{
		IfWinExist, ahk_exe vbs3_64_ptur.exe
		{
			Sleep, 30000
			global isGunner = true
			global isSwapped = ActivateWindow()
			MsgBox SwapWindow
		}
	}
}			

CheckCloseWindow()
{
	IfWinNotExist, ahk_exe vbs3_64.exe
	{
		global isSwapped = false
			MsgBox CheckCloseWindow
	}
}