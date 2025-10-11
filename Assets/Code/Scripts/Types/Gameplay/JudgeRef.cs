using System;

namespace Code.Scripts.Types.Gameplay
{
    [Flags]
    public enum JudgeRef
    {
        None = 0,
        SenechalAnscherius = 1,
        ConnetableGuillaumeDeBures = 2,
        PayenLeBouteiller = 4,
        MarechalSadon = 8,
        GautierGranier = 16,
        GeraudGranier = 32
    }
}
