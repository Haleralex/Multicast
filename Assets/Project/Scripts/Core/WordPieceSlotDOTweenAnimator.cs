using DG.Tweening;
using UnityEngine;
public class WordPieceSlotDOTweenAnimator : IWordPieceSlotAnimator
{
    public void ResetToDefaultCondition(WordPieceSlot wordPieceSlot)
    {
        var rectTransform = wordPieceSlot.rectTransform;

        // Убиваем предыдущие анимации на этом объекте
        rectTransform.DOKill();
    }

    public void SetClosestSlotAnimation(WordPieceSlot wordPieceSlot)
    {
        var rectTransform = wordPieceSlot.rectTransform;

        // Убиваем предыдущие анимации на этом объекте
        rectTransform.DOKill();

        // Короткая прикольная анимация: пульсация + покачивание
        var sequence = DOTween.Sequence();

        // Быстрый пульс масштаба
        sequence.Append(rectTransform.DOScale(1.15f, 0.1f).SetEase(Ease.OutBack))
                .Append(rectTransform.DOScale(1.05f, 0.1f).SetEase(Ease.InOutQuad))

                // Легкое покачивание
                .Join(rectTransform.DORotate(new Vector3(0, 0, 3f), 0.15f).SetEase(Ease.InOutSine))
                .Append(rectTransform.DORotate(new Vector3(0, 0, -3f), 0.3f).SetEase(Ease.InOutSine))
                .Append(rectTransform.DORotate(Vector3.zero, 0.15f).SetEase(Ease.InOutSine))

                // Возврат к нормальному размеру
                .Join(rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce));

        sequence.SetTarget(rectTransform);
    }
}
