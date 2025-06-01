using System;
using System.Collections.Generic;
using Core.Implementations;
using Core.Interfaces;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class WordPiecesPool : IDisposable
{
    private readonly IWordPiecesFactory wordPiecesFactory;
    private int initialPoolSize = 6;
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
        {
            Debug.LogError("WordPiecesPool is already initialized.");
            return;
        }
        await wordPiecesFactory.Initialize();
        for (int i = 0; i < initialPoolSize; i++)
        {
            pooledPieces.Add(CreateNewWordPieceAsync());
        }

        isInitialized = true;
    }
    private WordPiece CreateNewWordPieceAsync()
    {
        var wordPiece = wordPiecesFactory.Create();
        CreationCallback(wordPiece);
        return wordPiece;
    }

    private void CreationCallback(WordPiece wordPiece)
    {
        if(wordPiece == null)
        {
            Debug.LogError("WordPiece is null after creation.");
            return;
        }
        wordPiece.gameObject.SetActive(false);
    }
    private void GettingCallback(WordPiece wordPiece)
    {
        if(wordPiece == null)
        {
            Debug.LogError("WordPiece is null.");
            return;
        }
        wordPiece.gameObject.SetActive(true);
    }

    public WordPiece GetWordPiece()
    {
        foreach (var piece in pooledPieces)
        {
            if (!piece.gameObject.activeSelf)
            {
                piece.gameObject.SetActive(true);
                return piece;
            }
        }

        var newPiece = CreateNewWordPieceAsync();
        GettingCallback(newPiece);
        return newPiece;
    }

    public void ReturnWordPiece(WordPiece piece)
    {
        if (piece != null)
        {
            piece.gameObject.SetActive(false);
        }
    }

    public void Dispose()
    {
        pooledPieces.Clear();
    }
}
