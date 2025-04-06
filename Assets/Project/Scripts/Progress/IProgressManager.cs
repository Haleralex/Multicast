using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Progress
{
    /// <summary>
    /// This interface defines methods for saving and loading game progress.
    /// </summary>
    public interface IProgressManager
    {
        void SaveProgress(GameProgress progress);
        GameProgress LoadProgress();
    }
}
