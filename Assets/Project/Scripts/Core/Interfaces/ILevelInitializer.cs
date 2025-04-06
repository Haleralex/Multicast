using System.Collections;
using System.Collections.Generic;
using Progress;
using UnityEngine;

public interface ILevelInitializer
{
    void InitializeLevel(Dictionary<int, Dictionary<int, string>> correctMapping,
        Dictionary<int, string> missingSlots, GameProgress progress = null);
}
