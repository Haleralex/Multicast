using System.Collections.Generic;
using Core.Implementations;
using Cysharp.Threading.Tasks;
using UnityEngine;
using ZLinq;

public class WordPieceSlotsService : IWordPieceSlotsService
{
    private readonly WordPieceSlotsContainerPool _containerPool;
    private readonly List<WordPieceSlotsContainer> _activeContainers = new();

    public WordPieceSlotsService(WordPieceSlotsContainerPool containerPool)
    {
        _containerPool = containerPool;
    }

    public async UniTask<List<WordPieceSlot>> CreateWordPieceSlots(
        Dictionary<int, Dictionary<int, string>> correctMapping,
        Dictionary<int, string> missingSlots)
    {
        await _containerPool.Initialize();
        
        var wordPieceSlots = new List<WordPieceSlot>();
        
        foreach (var correctMap in correctMapping)
        {
            var container = _containerPool.GetContainerAsync();
            container.Initialize(correctMap.Value, missingSlots);
            _activeContainers.Add(container);
            wordPieceSlots.AddRange(container.ActiveWordPieceSlots.AsValueEnumerable().ToList());
        }
        
        return wordPieceSlots;
    }

    public async UniTask ResetSlots()
    {
        foreach (var container in _activeContainers)
        {
            _containerPool.ReturnContainer(container);
        }
        _activeContainers.Clear();
        await UniTask.CompletedTask;
    }
}