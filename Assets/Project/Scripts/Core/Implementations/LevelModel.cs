using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using System.Linq;
using System.Collections.Generic;
using Progress;
using Data;
public class LevelModel
{
    private readonly IProgressManager progressManager;

    private Dictionary<int, int> wordPiecesMappings = new();
    public IReadOnlyDictionary<int, int> WordPiecesMappings => wordPiecesMappings;

    public event Action<MappingUpdate> WordPiecesMappingChanged;
    private Dictionary<int, string> missingSlotsMapping = new();
    private Dictionary<int, Dictionary<int, string>> correctSlotsMapping = new();

    private List<string> guessedWords = new List<string>();
    public IReadOnlyList<string> GuessedWords => guessedWords;

    [Inject]
    public LevelModel(IProgressManager progressManager)
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

    private void SaveGuessedWords()
    {
        var currentProgress = GetCurrentProgress();
        currentProgress.GuessedWords = guessedWords;
        progressManager.SaveProgress(currentProgress);
    }

    public void FinishLevel()
    {
        var currentProgress = GetCurrentProgress();
        currentProgress.CurrentLevel++;
        currentProgress.WordPiecesMapping.Clear();
        progressManager.SaveProgress(currentProgress);
    }

    public async UniTask<HandledLevelData> GetCurrentLevelData()
    {
        var levelIndex = progressManager.LoadProgress().CurrentLevel % 4; // Assuming 4 levels for simplicity
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
            correctSlotsMapping.Clear();
            missingSlotsMapping.Clear();
            guessedWords.Clear();
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
            var handledLevelData = new HandledLevelData(correctSlotsMapping, missingSlotsMapping);
            return handledLevelData;
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Failed to deserialize LevelData JSON for level {levelIndex}: {ex.Message}");
            return null;
        }
    }

    public string GetWordForCompleteLevel()
    {
        var strBuilder = new System.Text.StringBuilder();
        foreach (var k in correctSlotsMapping)
        {
            foreach (var v in k.Value.Where(v => missingSlotsMapping.ContainsKey(v.Key)))
            {
                strBuilder.Append(v.Value);
            }
        }
        return strBuilder.ToString();
    }

    public void MapWordPieceToSlot(int wordPieceId, int slotId)
    {
        wordPiecesMappings[wordPieceId] = slotId;

        CheckForCompletedWord(slotId);

        WordPiecesMappingChanged?.Invoke(MappingUpdate.AddOrUpdate(wordPieceId, slotId));
    }

    private void CheckForCompletedWord(int lastAddedSlotId)
    {
        // Определяем, к какому слову относится этот слот
        foreach (var wordEntry in correctSlotsMapping)
        {
            int wordIndex = wordEntry.Key;
            var wordSlots = wordEntry.Value;

            // Проверяем, относится ли слот к текущему слову
            if (wordSlots.ContainsKey(lastAddedSlotId))
            {
                // Получаем все слоты для этого слова
                var requiredSlots = wordSlots.Keys.ToList();

                // Проверяем, все ли слоты этого слова заполнены
                bool isComplete = true;
                foreach (var slotId in requiredSlots.Where(a => missingSlotsMapping.ContainsKey(a)))
                {
                    // Если хотя бы один слот не заполнен, слово неполное
                    if (!wordPiecesMappings.ContainsValue(slotId))
                    {
                        isComplete = false;
                        break;
                    }
                }

                // Если все слоты заполнены, собираем и добавляем слово
                if (isComplete)
                {
                    // Собираем слово из фрагментов в правильном порядке
                    var word = new System.Text.StringBuilder();
                    foreach (var slot in requiredSlots.OrderBy(s => s))
                    {
                        // Найти wordPieceId который находится в этом слоте
                        int pieceId = wordPiecesMappings.FirstOrDefault(m => m.Value == slot).Key;

                        // Добавляем фрагмент к слову
                        if (wordSlots.TryGetValue(slot, out string fragment))
                        {
                            word.Append(fragment);
                        }
                    }

                    string completedWord = word.ToString();

                    // Проверяем, не было ли слово уже отгадано
                    if (!guessedWords.Contains(completedWord))
                    {
                        guessedWords.Add(completedWord);
                        SaveGuessedWords();
                        Debug.Log($"Отгадано новое слово: {completedWord}");
                    }

                    break;
                }
            }
        }
    }

    public void RemoveWordPieceMapping(int wordPieceId)
    {
        if (wordPiecesMappings.TryGetValue(wordPieceId, out var slotId))
        {
            // Удаляем маппинг
            wordPiecesMappings.Remove(wordPieceId);
            WordPiecesMappingChanged?.Invoke(MappingUpdate.Remove(wordPieceId));

            // Проверяем, какое слово было нарушено этим удалением
            CheckForBrokenWords(slotId);
        }
    }
    private void CheckForBrokenWords(int removedSlotId)
    {
        // Создаем список слов, которые нужно удалить из отгаданных
        List<string> wordsToRemove = new List<string>();

        // Проверяем, к какому слову относился удаленный слот
        foreach (var wordEntry in correctSlotsMapping)
        {
            int wordIndex = wordEntry.Key;
            var wordSlots = wordEntry.Value;

            // Если слот относился к этому слову
            if (wordSlots.ContainsKey(removedSlotId) && wordSlots.ContainsKey(removedSlotId))
            {
                // Получаем текущее слово, которое могло быть отгадано
                string currentWord = "";
                foreach (var slot in wordSlots.Keys.OrderBy(s => s))
                {
                    if (wordSlots.TryGetValue(slot, out string fragment))
                    {
                        currentWord += fragment;
                    }
                }

                // Проверяем, было ли это слово в списке отгаданных
                if (guessedWords.Contains(currentWord))
                {
                    wordsToRemove.Add(currentWord);
                    Debug.Log($"Слово '{currentWord}' больше не собрано полностью!");
                }
            }
        }

        // Удаляем нарушенные слова из списка отгаданных
        foreach (var wordToRemove in wordsToRemove)
        {
            guessedWords.Remove(wordToRemove);
        }
        SaveGuessedWords();
    }
    public void ResetWordPieceMappings()
    {
        wordPiecesMappings.Clear();
        WordPiecesMappingChanged?.Invoke(MappingUpdate.Clear());
    }


    public void InitializeFromProgress()
    {
        var progress = progressManager.LoadProgress();
        wordPiecesMappings = new Dictionary<int, int>(progress.WordPiecesMapping);
        guessedWords = new List<string>(progress.GuessedWords);
        WordPiecesMappingChanged?.Invoke(MappingUpdate.InitializeAll());
    }
}


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