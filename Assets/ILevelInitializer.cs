using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelInitializer
{
    void InitializeLevel(LevelData levelData, GameProgress progress = null);
}
