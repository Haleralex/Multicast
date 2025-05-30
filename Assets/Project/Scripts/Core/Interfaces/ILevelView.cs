using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILevelView
{
    event Action<WordPiece> WordPieceMoving;
    event Action<WordPiece> WordPieceSelected;
    event Action<WordPiece> WordPieceReleased;
    event Action<WordPiece> WordPieceDoubleClicked;
    event Action ValidateLevelPressed;
    event Action NextLevelPressed;
    event Action GoToMenuPressed;
    void UpdateUIFromMappings(IReadOnlyDictionary<int, int> mappings);
    void SetOccupationCondition(MappingUpdate mappings, IReadOnlyDictionary<int, int> currentMappings);
    WordPieceSlot FindClosestEmptySlot(WordPiece wordPiece, float maxDistance = 10f);
    void SetUIElements(List<WordPieceSlot> slots, List<WordPiece> pieces);
    void ReturnWordPieceToInitialPosition(WordPiece wordPiece);
    IEnumerable<WordPiece> GetAllWordPieces();
    void ResetAllWordPiecesToInitialPositions();
    void ShowLevelCompletedMessage(IReadOnlyList<string> words);
}
