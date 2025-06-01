using DG.Tweening;
using UnityEngine;
using System.Collections.Generic;

public class WordPieceSlotDOTweenAnimator : IWordPieceSlotAnimator
{
    private Dictionary<WordPieceSlot, SlotInitialState> initialStates = new Dictionary<WordPieceSlot, SlotInitialState>();
    private Sequence sequence;

    private struct SlotInitialState
    {
        public Vector3 scale;
        public Vector3 rotation;
        public Vector3 position;
    }

    public void ClearStates()
    {
        // Очищаем все сохраненные состояния
        initialStates.Clear();
        
        // Убиваем текущую последовательность анимаций, если она есть
        sequence?.Kill();
        sequence = null;
    }

    public void ResetToDefaultCondition(WordPieceSlot wordPieceSlot)
    {
        var rectTransform = wordPieceSlot.rectTransform;

        // Убиваем предыдущие анимации на этом объекте
        rectTransform.DOKill();
        sequence?.Kill();

        // Восстанавливаем изначальные значения
        if (initialStates.TryGetValue(wordPieceSlot, out var initialState))
        {
            rectTransform.localScale = initialState.scale;
            rectTransform.localEulerAngles = initialState.rotation;
            rectTransform.localPosition = initialState.position;
        }
        else
        {
            // Если состояние не сохранено, устанавливаем дефолтные значения
            rectTransform.localScale = Vector3.one;
            rectTransform.localEulerAngles = Vector3.zero;
        }
    }

    public void SetClosestSlotAnimation(WordPieceSlot wordPieceSlot)
    {
        var rectTransform = wordPieceSlot.rectTransform;
        
        // Сохраняем изначальное состояние перед анимацией
        if (!initialStates.ContainsKey(wordPieceSlot))
        {
            initialStates[wordPieceSlot] = new SlotInitialState
            {
                scale = rectTransform.localScale,
                rotation = rectTransform.localEulerAngles,
                position = rectTransform.localPosition
            };
        }

        sequence?.Kill();
        // Убиваем предыдущие анимации на этом объекте
        rectTransform.DOKill();
        
        // Короткая прикольная анимация: пульсация + покачивание
        sequence = DOTween.Sequence();
        // Быстрый пульс масштаба
        sequence.Append(rectTransform.DOScale(1.15f, 0.1f).SetEase(Ease.OutBack))
                .Append(rectTransform.DOScale(1.05f, 0.1f).SetEase(Ease.InOutQuad))

                // Легкое покачивание
                .Join(rectTransform.DORotate(new Vector3(0, 0, 3f), 0.15f).SetEase(Ease.InOutSine))
                .Append(rectTransform.DORotate(new Vector3(0, 0, -3f), 0.3f).SetEase(Ease.InOutSine))
                .Append(rectTransform.DORotate(Vector3.zero, 0.15f).SetEase(Ease.InOutSine))

                // Возврат к нормальному размеру
                .Join(rectTransform.DOScale(1f, 0.2f).SetEase(Ease.OutBounce))
                
                // Зацикливаем анимацию
                .SetLoops(-1, LoopType.Restart);

        sequence.SetTarget(rectTransform);
    }
}
