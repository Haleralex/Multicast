using UnityEngine;

public interface IWordPieceSlotAnimator
{
    void ClearStates();
    void SetClosestSlotAnimation(WordPieceSlot wordPieceSlot);
    void ResetToDefaultCondition(WordPieceSlot wordPieceSlot);
}
