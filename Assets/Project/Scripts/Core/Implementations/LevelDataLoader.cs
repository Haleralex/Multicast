using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Data;
using Newtonsoft.Json;
using Progress;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;

public class LevelDataLoader : ILevelDataLoader
{
    private readonly IProgressManager progressManager;

    [Inject]
    public LevelDataLoader(IProgressManager progressManager)
    {
        this.progressManager = progressManager;
    }

    public async UniTask<HandledLevelData> LoadCurrentLevelDataAsync()
    {
        int levelAmount = await GetLevelAssetsCountAsync();
        var levelIndex = progressManager.LoadProgress().CurrentLevel % levelAmount;
        var handle = Addressables.LoadAssetAsync<TextAsset>($"LevelData_{levelIndex}");
        await handle.ToUniTask();

        TextAsset jsonAsset = handle.Result;
        if (jsonAsset == null)
        {
            Debug.LogError($"Failed to load LevelData JSON for level {levelIndex}");
            return null;
        }

        try
        {
            LevelData levelData = JsonConvert.DeserializeObject<LevelData>(jsonAsset.text);
            
            var correctSlotsMapping = new Dictionary<int, Dictionary<int, string>>();
            var missingSlotsMapping = new Dictionary<int, string>();
            
            var fullWordIndex = 0;
            var baseIndex = 0;
            
            foreach (var word in levelData.Words)
            {
                correctSlotsMapping[fullWordIndex] = new Dictionary<int, string>();
                for (int i = 0; i < word.Clusters.Count; i++)
                {
                    correctSlotsMapping[fullWordIndex].Add(baseIndex + i, word.Clusters[i]);

                    if (word.MissingClusters.Contains(i))
                    {
                        missingSlotsMapping.Add(baseIndex + i, word.Clusters[i]);
                    }
                }
                fullWordIndex++;
                baseIndex += word.Clusters.Count;
            }

            Addressables.Release(handle);
            return new HandledLevelData(correctSlotsMapping, missingSlotsMapping);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to deserialize LevelData JSON for level {levelIndex}: {ex.Message}");
            return null;
        }
    }

    public async UniTask<int> GetLevelAssetsCountAsync()
    {
        try
        {
            var operation = Addressables.LoadResourceLocationsAsync("Game_Level", typeof(TextAsset));
            await operation.ToUniTask();

            int count = operation.Result.Count;
            Addressables.Release(operation);

            return count > 0 ? count : 1;
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to get level assets count: {ex.Message}");
            return 4;
        }
    }
}