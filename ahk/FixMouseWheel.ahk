WheelUp::
WheelDown::SendInput % "{" ((A_TimeSincePriorHotkey < 20 && A_PriorHotkey ~= "WheelDown|WheelUp") ? hk
 : hk := A_ThisHotkey) "}"