using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Zenject;
using ZLinq;
using System.Collections.Generic;
using Progress;
using Data;
public class LevelModel
{
    private readonly IProgressManager progressManager;
    private readonly ILevelDataLoader levelDataLoader;

    private Dictionary<int, int> wordPiecesMappings = new();
    public IReadOnlyDictionary<int, int> WordPiecesMappings => wordPiecesMappings;

    public event Action<MappingUpdate> WordPiecesMappingChanged;
    private Dictionary<int, string> missingSlotsMapping = new();
    private Dictionary<int, Dictionary<int, string>> correctSlotsMapping = new();

    private List<string> guessedWords = new List<string>();
    public IReadOnlyList<string> GuessedWords => guessedWords;

    [Inject]
    public LevelModel(IProgressManager progressManager, ILevelDataLoader levelDataLoader)
    {
        this.progressManager = progressManager;
        this.levelDataLoader = levelDataLoader;
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
        var handledLevelData = await levelDataLoader.LoadCurrentLevelDataAsync();

        if (handledLevelData != null)
        {
            correctSlotsMapping = new Dictionary<int, Dictionary<int, string>>(handledLevelData.CorrectSlotsMapping);
            missingSlotsMapping = new Dictionary<int, string>(handledLevelData.MissingSlotsMapping);
            guessedWords.Clear();
        }

        return handledLevelData;
    }
    public string GetWordForCompleteLevel()
    {
        var strBuilder = new System.Text.StringBuilder();
        foreach (var k in correctSlotsMapping)
        {
            foreach (var v in k.Value.AsValueEnumerable().Where(v => missingSlotsMapping.ContainsKey(v.Key)))
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
        foreach (var wordEntry in correctSlotsMapping)
        {
            int wordIndex = wordEntry.Key;
            var wordSlots = wordEntry.Value;

            if (wordSlots.ContainsKey(lastAddedSlotId))
            {
                // Получаем все слоты для этого слова
                var requiredSlots = wordSlots.Keys.AsValueEnumerable().ToList();

                bool isComplete = true;
                foreach (var slotId in requiredSlots.AsValueEnumerable().Where(a => missingSlotsMapping.ContainsKey(a)))
                {
                    if (!wordPiecesMappings.ContainsValue(slotId))
                    {
                        isComplete = false;
                        break;
                    }
                }

                if (isComplete)
                {
                    var word = new System.Text.StringBuilder();
                    foreach (var slot in requiredSlots.AsValueEnumerable().OrderBy(s => s))
                    {
                        int pieceId = wordPiecesMappings.AsValueEnumerable().FirstOrDefault(m => m.Value == slot).Key;

                        if (wordSlots.TryGetValue(slot, out string fragment))
                        {
                            word.Append(fragment);
                        }
                    }

                    string completedWord = word.ToString();

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
            wordPiecesMappings.Remove(wordPieceId);
            WordPiecesMappingChanged?.Invoke(MappingUpdate.Remove(wordPieceId,slotId));

            CheckForBrokenWords(slotId);
        }
    }
    private void CheckForBrokenWords(int removedSlotId)
    {
        List<string> wordsToRemove = new List<string>();

        foreach (var wordEntry in correctSlotsMapping)
        {
            int wordIndex = wordEntry.Key;
            var wordSlots = wordEntry.Value;

            if (wordSlots.ContainsKey(removedSlotId) && wordSlots.ContainsKey(removedSlotId))
            {
                string currentWord = "";
                foreach (var slot in wordSlots.Keys.AsValueEnumerable().OrderBy(s => s))
                {
                    if (wordSlots.TryGetValue(slot, out string fragment))
                    {
                        currentWord += fragment;
                    }
                }

                if (guessedWords.Contains(currentWord))
                {
                    wordsToRemove.Add(currentWord);
                    Debug.Log($"Слово '{currentWord}' больше не собрано полностью!");
                }
            }
        }

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
    
    public bool IsLevelFull()
    {
        return guessedWords.Count == correctSlotsMapping.Count;
    }
}