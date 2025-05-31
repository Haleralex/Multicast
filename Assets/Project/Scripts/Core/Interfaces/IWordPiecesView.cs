using System;
using System.Collections.Generic;
using UnityEngine;

public interface IWordPiecesView
{
    event Action<WordPiece> WordPieceSelected;
    event Action<WordPiece> WordPieceMoving;
    event Action<WordPiece> WordPieceReleased;
    event Action<WordPiece> WordPieceDoubleClicked;

    void SetWordPieces(List<WordPiece> pieces);
    void ReturnWordPieceToInitialPosition(WordPiece wordPiece);
    void ResetAllWordPieces();
    IEnumerable<WordPiece> GetAllWordPieces();
    bool TryGetWordPiece(int index, out WordPiece wordPiece);
    void ChangeWordPieceParent(int wordPieceIndex,
        Transform parent, bool nullifyPosition = false);
}
