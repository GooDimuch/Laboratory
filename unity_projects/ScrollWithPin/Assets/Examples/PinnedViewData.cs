using System;

public class PinnedViewData : ICloneable
{
    public string Text;
    public object Clone() => MemberwiseClone();
}