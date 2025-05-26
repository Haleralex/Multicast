using System;
using System.Collections.Generic;
using UnityEngine;

namespace Core.Interfaces
{
    public interface IWordPieceAnimator
    {
        void PlayAppearAnimation(GameObject target);
        void PlayDisappearAnimation(GameObject target, Action onComplete = null);
        void PlaySequentialAppearAnimation(List<GameObject> activeSlots);
    }
}