using System.Collections.Generic;
using System.Linq;
using Core.Interfaces;
using UnityEngine;

public class WordPieceSlotsContainer : MonoBehaviour
{
    [SerializeField] private List<WordPieceSlot> slots;
    [SerializeField] private bool animateSlots = true;
    [SerializeField] private float slotAnimationDelay = 0.05f;

    private IWordPieceAnimator animator;

    [Zenject.Inject]
    public void Construct(IWordPieceAnimator animator)
    {
        this.animator = animator;
    }
    public void Initialize(Dictionary<int, string> allFragments,
    Dictionary<int, string> missingFragments)
    {
        ResetState();
        slots.ForEach(a => a.gameObject.SetActive(false));
        var index = 0;
        foreach (var k in allFragments)
        {
            var slot = slots[index++];
            if (!missingFragments.ContainsKey(k.Key))
            {
                slot.Initialize(k.Key, k.Value);
            }
            else
            {
                slot.Initialize(k.Key);
            }
        }

        if (animateSlots && animator != null)
        {
            AnimateSlotsAppearance();
        }
    }

    private void AnimateSlotsAppearance()
    {
       /*  var activeSlots = ActiveWordPieceSlots.Select(s => s.gameObject).ToList();
        animator.PlaySequentialAppearAnimation(activeSlots, slotAnimationDelay); */
    }

    public void ResetState()
    {
        slots.ForEach(a => a.gameObject.SetActive(false));
        
    }

    public IReadOnlyCollection<WordPieceSlot> ActiveWordPieceSlots
        => slots.Where(a => a.gameObject.activeSelf).ToList();
}
