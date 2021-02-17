; прицел
x = 959
y = 539

w = 3
h = 3
Color = 0xFFFFFF
; WS_EX_TRANSPARENT := 0x20
; WS_EX_LAYERED := 0x80000
Gui, +AlwaysOnTop -Caption +ToolWindow +LastFound
Gui, Color, % Color
Gui, Show, x%x% y%y% w%w% h%h% NA
WinSet, ExStyle, % "+" WS_EX_LAYERED|WS_EX_TRANSPARENT