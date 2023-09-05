using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Extensions;

public class Utils
{
    public static bool IsSameOrSubclass(Type potentialBase, Type type) =>
        type.IsSubclassOf(potentialBase) || type == potentialBase;
}