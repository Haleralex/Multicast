using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class WordPiecesPool
{
    private WordPiecesFactory wordPiecesFactory;
    private int initialPoolSize = 12;
    
    private List<WordPiece> pooledPieces = new();
    
    [Inject]
    public WordPiecesPool(WordPiecesFactory factory)
    {
        this.wordPiecesFactory = factory;
        
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewWordPiece();
        }
    }
    
    private WordPiece CreateNewWordPiece()
    {
        var wordPiece = wordPiecesFactory.Create();
        wordPiece.gameObject.SetActive(false);
        pooledPieces.Add(wordPiece);
        return wordPiece;
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
        
        return CreateNewWordPiece();
    }
    
    public void ReturnWordPiece(WordPiece piece)
    {
        if (piece != null)
        {
            piece.gameObject.SetActive(false);
        }
    }
}
