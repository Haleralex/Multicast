using System.Collections.Generic;
using Core.Interfaces;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Core.Implementations
{
    public class WordPieceDOTweenAnimator : IWordPieceAnimator
    {
        private readonly float appearDuration = 0.1f;
        private readonly float disappearDuration = 0.2f;

        public void PlayAppearAnimation(GameObject target)
        {
            // Сбрасываем состояние перед анимацией
            target.transform.localScale = Vector3.zero;
            target.SetActive(true);

            // Запускаем анимацию появления
            target.transform.DOScale(Vector3.one, appearDuration)
                .SetEase(Ease.OutBack);
        }

        public void PlayDisappearAnimation(GameObject target, System.Action onComplete = null)
        {
            // Запускаем анимацию исчезновения
            target.transform.DOScale(Vector3.zero, disappearDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    target.SetActive(false);
                    onComplete?.Invoke();
                });
        }

        public void PlaySequentialAppearAnimation(List<GameObject> targets, float delayBetween = 0.1f)
        {
            if (targets == null || targets.Count == 0)
                return;

            // Сначала сбрасываем все состояния
            foreach (var target in targets)
            {
                target.transform.GetChild(1).localScale = Vector3.zero;
                target.SetActive(true);
            }

            // Создаем последовательность
            Sequence sequence = DOTween.Sequence();

            for (int i = 0; i < targets.Count; i++)
            {
                GameObject target = targets[i];

                // Добавляем задержку перед каждым элементом (кроме первого)
                if (i > 0)
                {
                    sequence.AppendInterval(delayBetween);
                }

                // Анимируем элемент в последовательности
                sequence.Append(target.transform.GetChild(1).DOScale(Vector3.one, appearDuration)
                    .SetEase(Ease.OutBack));
            }

            // Запускаем последовательность
            sequence.Play();
        }
    }
}