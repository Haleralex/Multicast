using UnityEngine;

public interface IWordPieceSlotAnimator
{
    void SetClosestSlotAnimation(WordPieceSlot wordPieceSlot);
    void ResetToDefaultCondition(WordPieceSlot wordPieceSlot);
}
