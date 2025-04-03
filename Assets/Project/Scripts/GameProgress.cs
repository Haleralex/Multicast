using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameProgress
{
    public int CurrentLevel;
    public Dictionary<int, int> ClusterMapping = new();
}
