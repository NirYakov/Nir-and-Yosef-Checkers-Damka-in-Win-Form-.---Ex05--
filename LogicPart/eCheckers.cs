using System;

namespace LogicPart
{
    [Flags]
    public enum eCheckers : byte 
    {
        Non = 0,
        CheckerO = 1,
        CheckerX = 2,
        CheckerU = 4,
        CheckerK = 8
    }
}