using System.Collections.Generic;
using ZLinq;
using Core.Interfaces;
using UnityEngine;

public class WordPieceSlotsContainer : MonoBehaviour
{
    [SerializeField] private List<WordPieceSlot> slots;

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
    }

    public void ResetState()
    {
        slots.ForEach(a => a.gameObject.SetActive(false));
    }

    public IReadOnlyCollection<WordPieceSlot> ActiveWordPieceSlots
        => slots.AsValueEnumerable().Where(a => a.gameObject.activeSelf).ToList();
}
