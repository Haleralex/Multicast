using System.Collections;
using System.Collections.Generic;
using ZLinq;
using Core.Implementations;
using Progress;
using Utils;
using Zenject;
using Core.Interfaces;
using System.Linq;
using Cysharp.Threading.Tasks;
using Unity.Android.Gradle.Manifest;
using UnityEngine.UI;
using UnityEngine;

public class LevelInitializer : ILevelInitializer, IInitializable
{
    [Inject] private readonly WordPieceSlotsContainerPool containerPool; // Заменили фабрику на пул

    /*     [Inject] private readonly IWordPieceSlotsContainerFactory containerFactory;
     */
    private List<WordPieceSlotsContainer> wordPieceSlotsContainer = new();

    [Inject] private readonly ILevelView levelView;
    [Inject] private readonly LevelModel levelModel;
    [Inject] private readonly WordPiecesPool wordPiecesPool;

    [Inject] private readonly IWordPieceAnimator animator;

    private List<WordPiece> activeWordPieces = new List<WordPiece>();

    public async void InitializeLevel(
        Dictionary<int, Dictionary<int, string>> correctMapping,
        Dictionary<int, string> missingSlots, GameProgress progress = null
        )
    {
        foreach (var container in wordPieceSlotsContainer)
        {
            if (container != null)
                containerPool.ReturnContainer(container);
        }
        wordPieceSlotsContainer.Clear();
        wordPieceSlotsContainer.Add(await containerPool.GetContainerAsync());
        wordPieceSlotsContainer.Add(await containerPool.GetContainerAsync());
        wordPieceSlotsContainer.Add(await containerPool.GetContainerAsync());
        wordPieceSlotsContainer.Add(await containerPool.GetContainerAsync());



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
            wordPieceSlots.AddRange(wordPieceSlotsContainer[wordIndex].ActiveWordPieceSlots.AsValueEnumerable().ToList());
            wordIndex++;
        }
        var shuffledMap = correctMapping.Values.AsValueEnumerable().ToList();
        shuffledMap.ShuffleUnity();
        
        foreach (var correctMap in shuffledMap)
        {
            var strings = correctMap.AsValueEnumerable().ToList();
            strings.Shuffle();
            var missingPieces = strings.AsValueEnumerable().Where(a => missingSlots.ContainsKey(a.Key)).ToList();
            foreach (var k in missingPieces)
            {
                var availableWordPiece = await wordPiecesPool.GetWordPieceAsync();
                availableWordPiece.Initialize(k.Value, k.Key);
                activeWordPieces.Add(availableWordPiece);
                wordPieces.Add(availableWordPiece);
            }
        }
        levelView.SetUIElements(wordPieceSlots, wordPieces);
        animator.PlaySequentialAppearAnimation(wordPieces.Select(a => a.gameObject).ToList(), 0.025f);
        if (progress != null && progress.WordPiecesMapping.Count > 0)
        {
            levelModel.InitializeFromProgress();
        }
    }

    public async void Initialize()
    {
        await containerPool.Initialize();
    }
}
