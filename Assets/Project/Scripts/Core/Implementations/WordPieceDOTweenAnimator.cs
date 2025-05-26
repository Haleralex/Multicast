using System.Collections.Generic;
using Core.Interfaces;
using DG.Tweening;
using UnityEngine;
using Zenject;

namespace Core.Implementations
{
    public class WordPieceDOTweenAnimator : IWordPieceAnimator
    {
        private float AppearDuration => animationsConfig.wordPieceAppearDuration;
        private float DelayBetween => animationsConfig.delayBetween;
        private readonly float disappearDuration = 0.2f;

        [Inject] private readonly AnimationsConfig animationsConfig;

        public void PlayAppearAnimation(GameObject target)
        {
            target.transform.localScale = Vector3.zero;
            target.SetActive(true);

            target.transform.DOScale(Vector3.one, AppearDuration)
                .SetEase(Ease.OutBack);
        }

        public void PlayDisappearAnimation(GameObject target, System.Action onComplete = null)
        {
            target.transform.DOScale(Vector3.zero, disappearDuration)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    target.SetActive(false);
                    onComplete?.Invoke();
                });
        }

        public void PlaySequentialAppearAnimation(List<GameObject> targets)
        {
            if (targets == null || targets.Count == 0)
                return;

            foreach (var target in targets)
            {
                target.transform.GetChild(1).localScale = Vector3.zero;
                target.SetActive(true);
            }

            Sequence sequence = DOTween.Sequence();

            for (int i = 0; i < targets.Count; i++)
            {
                GameObject target = targets[i];

                if (i > 0)
                {
                    sequence.AppendInterval(DelayBetween);
                }

                sequence.Append(target.transform.GetChild(1).DOScale(Vector3.one, AppearDuration)
                    .SetEase(Ease.OutBack));
            }

            sequence.Play();
        }
    }
}