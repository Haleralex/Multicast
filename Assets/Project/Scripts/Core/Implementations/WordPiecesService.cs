using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Utils;
using ZLinq;

public class WordPiecesService : IWordPiecesService
{
    private readonly WordPiecesPool _wordPiecesPool;
    private readonly List<WordPiece> _activeWordPieces = new();

    public WordPiecesService(WordPiecesPool wordPiecesPool)
    {
        _wordPiecesPool = wordPiecesPool;
    }

    public async UniTask<List<WordPiece>> CreateWordPieces(
        Dictionary<int, Dictionary<int, string>> correctMapping,
        Dictionary<int, string> missingSlots)
    {
        await _wordPiecesPool.Initialize();
        
        var wordPieces = new List<WordPiece>();
        var shuffledMap = GetShuffledMapping(correctMapping);

        foreach (var correctMap in shuffledMap)
        {
            var missingPieces = GetMissingPieces(correctMap, missingSlots);
            var pieces = CreateWordPiecesFromMissingPieces(missingPieces);
            wordPieces.AddRange(pieces);
        }
        
        return wordPieces;
    }

    public async UniTask ResetWordPieces()
    {
        foreach (var wordPiece in _activeWordPieces)
        {
            _wordPiecesPool.ReturnWordPiece(wordPiece);
        }
        _activeWordPieces.Clear();
        await UniTask.CompletedTask;
    }

    private List<Dictionary<int, string>> GetShuffledMapping(Dictionary<int, Dictionary<int, string>> correctMapping)
    {
        var shuffledMap = correctMapping.Values.AsValueEnumerable().ToList();
        shuffledMap.ShuffleUnity();
        return shuffledMap;
    }

    private List<KeyValuePair<int, string>> GetMissingPieces(
        Dictionary<int, string> correctMap, 
        Dictionary<int, string> missingSlots)
    {
        var strings = correctMap.AsValueEnumerable().ToList();
        strings.Shuffle();
        return strings.AsValueEnumerable()
            .Where(a => missingSlots.ContainsKey(a.Key))
            .ToList();
    }

    private List<WordPiece> CreateWordPiecesFromMissingPieces(
        List<KeyValuePair<int, string>> missingPieces)
    {
        var pieces = new List<WordPiece>();
        
        foreach (var missingPiece in missingPieces)
        {
            var wordPiece = _wordPiecesPool.GetWordPiece();
            wordPiece.Initialize(missingPiece.Value, missingPiece.Key);
            _activeWordPieces.Add(wordPiece);
            pieces.Add(wordPiece);
        }
        
        return pieces;
    }
}