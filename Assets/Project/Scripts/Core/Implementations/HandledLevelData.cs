using System.Collections.Generic;
using UnityEngine;

public class HandledLevelData
{
    public Dictionary<int, Dictionary<int, string>> CorrectSlotsMapping { get; private set; }
    public Dictionary<int, string> MissingSlotsMapping { get; private set; }

    public HandledLevelData(Dictionary<int, Dictionary<int, string>> correctSlotsMapping,
        Dictionary<int, string> missingSlotsMapping)
    {
        CorrectSlotsMapping = correctSlotsMapping;
        MissingSlotsMapping = missingSlotsMapping;
    }
}