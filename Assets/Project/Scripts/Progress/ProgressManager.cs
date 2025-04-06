using System;
using Newtonsoft.Json;
using UnityEngine;

namespace Progress
{
    /// <summary>
    /// This class is responsible for managing the game progress using PlayerPrefs.
    /// </summary>
    /// <remarks>
    /// It implements the IProgressManager interface to provide methods for saving and loading game progress.
    /// </remarks>
    public class ProgressManager : IProgressManager
    {
        public ProgressManager()
        {
            if (!PlayerPrefs.HasKey("LevelProgress"))
            {
                SaveProgress(new GameProgress());
            }
        }

        public void SaveProgress(GameProgress gameProgress)
        {
            var stringData = JsonConvert.SerializeObject(gameProgress);
            if (!string.IsNullOrEmpty(stringData))
            {
                Debug.LogError("Progress data already exists in PlayerPrefs. Overwriting it.");
            }
            PlayerPrefs.SetString("LevelProgress", stringData);
        }

        public GameProgress LoadProgress()
        {
            var stringData = PlayerPrefs.GetString("LevelProgress");
            if (string.IsNullOrEmpty(stringData))
            {
                Debug.LogError("No progress data found in PlayerPrefs.");
                return null;
            }
            var currentLevelProgress = JsonConvert.DeserializeObject<GameProgress>(stringData);

            return currentLevelProgress;
        }
    }
}