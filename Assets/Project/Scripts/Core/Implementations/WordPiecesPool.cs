using System.Collections.Generic;
using Core.Implementations;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class WordPiecesPool : IInitializable
{
    private readonly IWordPiecesFactory wordPiecesFactory;
    private readonly IWordPieceAnimator animator;
    private int initialPoolSize = 12;
    private bool isInitialized = false;

    private List<WordPiece> pooledPieces = new();

    [Inject]
    public WordPiecesPool(IWordPiecesFactory factory, IWordPieceAnimator animator)
    {
        this.wordPiecesFactory = factory;
        this.animator = animator;
    }

    public async UniTask Initialize()
    {
        if (isInitialized)
            return;

        await wordPiecesFactory.Initialize();

        for (int i = 0; i < initialPoolSize; i++)
        {
            await CreateNewWordPieceAsync();
        }

        isInitialized = true;
    }
    private async UniTask<WordPiece> CreateNewWordPieceAsync()
    {
        var wordPiece = await wordPiecesFactory.CreateAsync();
        if (wordPiece != null)
        {
            wordPiece.gameObject.SetActive(false);
            pooledPieces.Add(wordPiece);
        }
        return wordPiece;
    }

    public async UniTask<WordPiece> GetWordPieceAsync()
    {
        while (!isInitialized)
        {
            await UniTask.Yield();
        }

        if (!isInitialized)
            await Initialize();

        foreach (var piece in pooledPieces)
        {
            if (!piece.gameObject.activeSelf)
            {
                animator.PlayAppearAnimation(piece.gameObject);
                return piece;
            }
        }

        var newPiece = await CreateNewWordPieceAsync();
        animator.PlayAppearAnimation(newPiece.gameObject);
        return newPiece;
    }

    public void ReturnWordPiece(WordPiece piece)
    {
        if (piece != null)
        {
            animator.PlayDisappearAnimation(piece.gameObject);
        }
    }

    async void IInitializable.Initialize()
    {
        await Initialize();
    }
}
