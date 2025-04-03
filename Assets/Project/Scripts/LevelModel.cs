using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
using System.Linq;
public class LevelModel
{
    private readonly ProgressManager progressManager;

    private string fullWordToCompleteLevel;

    [Inject]
    public LevelModel(ProgressManager progressManager)
    {
        this.progressManager = progressManager;
    }

    public GameProgress GetCurrentProgress()
    {
        var currentProgress = progressManager.LoadProgress();
        return currentProgress;
    }

    public void UpdateProgress(GameProgress newProgress)
    {
        progressManager.SaveProgress(newProgress);
    }

    public async UniTask<LevelData> GetLevelData(int levelIndex)
    {
        levelIndex %= 4;
        var handle = Addressables.LoadAssetAsync<TextAsset>($"LevelData_{levelIndex}");
        TextAsset jsonAsset = await handle.Task;

        if (jsonAsset == null)
        {
            Debug.LogError($"Failed to load LevelData JSON for level {levelIndex}");
            return null;
        }

        try
        {
            LevelData levelData = JsonConvert.DeserializeObject<LevelData>(jsonAsset.text);
            fullWordToCompleteLevel = string.Join("", levelData.Words.SelectMany(w => w.Key));
            Addressables.Release(handle);
            return levelData;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to deserialize LevelData JSON for level {levelIndex}: {ex.Message}");
            return null;
        }
    }

    public string GetWordForCompleteLevel()
    {
        return fullWordToCompleteLevel;
    }
}
