using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Progress;
using Unity.VisualScripting;
using UnityEngine;
using Utils;
using Zenject;

public class LevelInitializer : ILevelInitializer
{
    [Inject] private readonly List<WordPieceSlotsContainer> wordPieceSlotsContainer;
    [Inject] private readonly ILevelView levelView;
    [Inject] private readonly LevelModel levelModel;
    [Inject] private readonly WordPiecesPool wordPiecesPool;
    private List<WordPiece> activeWordPieces = new List<WordPiece>();

    public void InitializeLevel(
        Dictionary<int, Dictionary<int, string>> correctMapping,
        Dictionary<int, string> missingSlots, GameProgress progress = null
        )
    {
        foreach (var wordPiece in activeWordPieces)
        {
            wordPiecesPool.ReturnWordPiece(wordPiece);
        }
        activeWordPieces.Clear();
        var wordPieces = new List<WordPiece>();
        var wordPieceSlots = new List<WordPieceSlot>();
        var wordIndex = 0;
        foreach (var correctMap in correctMapping)
        {
            wordPieceSlotsContainer[wordIndex]
                .Initialize(correctMapping[correctMap.Key], missingSlots);
            wordPieceSlots.AddRange(wordPieceSlotsContainer[wordIndex].ActiveWordPieceSlots.ToList());
            wordIndex++;
        }
        var shuffledMap = correctMapping.Values.ToList();
        shuffledMap.ShuffleUnity();
        foreach (var correctMap in shuffledMap)
        {
            var strings = correctMap.ToList();
            strings.Shuffle();
            var missingPieces = strings.Where(a => missingSlots.ContainsKey(a.Key)).ToList();
            foreach (var k in missingPieces)
            {
                var availableWordPiece = wordPiecesPool.GetWordPiece();
                availableWordPiece.Initialize(k.Value, k.Key);
                activeWordPieces.Add(availableWordPiece);
                wordPieces.Add(availableWordPiece);
            }
        }
        levelView.SetUIElements(wordPieceSlots, wordPieces);

        if (progress != null && progress.WordPiecesMapping.Count > 0)
        {
            levelModel.InitializeFromProgress();
        }
    }
}
