using System.Collections.Generic;
using Progress;
using UnityEngine;

public interface IProgressRestorer
{
    void RestoreProgress(GameProgress progress,
        List<WordPiece> wordPieces, List<WordPieceSlot> slots);
}