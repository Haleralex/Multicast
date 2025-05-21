using System;
using System.Collections.Generic;
using Core.Implementations;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class WordPiecesPool : IInitializable, IDisposable
{
    private readonly IWordPiecesFactory wordPiecesFactory;
    private int initialPoolSize = 12;
    private bool isInitialized = false;

    private List<WordPiece> pooledPieces = new();

    [Inject]
    public WordPiecesPool(IWordPiecesFactory factory)
    {
        this.wordPiecesFactory = factory;
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
        foreach (var piece in pooledPieces)
        {
            if (!piece.gameObject.activeSelf)
            {
                piece.gameObject.SetActive(true);
                return piece;
            }
        }

        var newPiece = await CreateNewWordPieceAsync();
        newPiece.gameObject.SetActive(true);
        return newPiece;
    }

    public void ReturnWordPiece(WordPiece piece)
    {
        if (piece != null)
        {
            piece.gameObject.SetActive(false);
        }
    }

    async void IInitializable.Initialize()
    {
        await Initialize();
    }

    public void Dispose()
    {
        pooledPieces.Clear();
    }
}
