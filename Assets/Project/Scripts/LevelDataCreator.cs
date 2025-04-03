using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft;
using Newtonsoft.Json;
public class LevelDataCreator
{
    public void GenerateLevelData(string levelName, string[] words)
    {
        Dictionary<string, List<string>> levelDataWords = new();
        foreach (var word in words)
        {
            List<string> clusters = new List<string>();
            int index = 0;
            while (index < word.Length)
            {
                int remainingLength = word.Length - index;

                int clusterSize;
                if (remainingLength == 2 || remainingLength == 3 || remainingLength == 4)
                {
                    clusterSize = remainingLength;
                }
                else
                {
                    int[] clusterSizes = { 2, 3, 4 };
                    clusterSize = clusterSizes[Random.Range(0, clusterSizes.Length)];
                }

                clusters.Add(word.Substring(index, clusterSize));
                index += clusterSize;
            }

            levelDataWords[word] = clusters;
        }

        LevelData levelData = new LevelData
        {
            Words = levelDataWords
        };

        string json = JsonConvert.SerializeObject(levelData);
        string path = Path.Combine(Application.dataPath, $"{levelName}_LevelData.json");
        File.WriteAllText(path, json);

        Debug.Log($"LevelData для уровня '{levelName}' создан и сохранен в {path}");
    }    
}
