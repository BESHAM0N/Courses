using System;

[Flags]
public enum IgnoreAxisType : byte
{
    None = 0,
    X = 1,
    Y = 2,
    Z = 4
}
