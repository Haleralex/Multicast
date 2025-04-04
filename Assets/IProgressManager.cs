using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IProgressManager
{
    void SaveProgress(GameProgress progress);
    GameProgress LoadProgress();
}
