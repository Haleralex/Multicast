using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WordPieceSlotsContainer : MonoBehaviour
{
    [SerializeField] private List<WordPieceSlot> slots;
    public void Initialize(Dictionary<int, string> allFragments,
    Dictionary<int, string> missingFragments)
    {
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


    public IReadOnlyCollection<WordPieceSlot> ActiveWordPieceSlots
        => slots.Where(a => a.gameObject.activeSelf).ToList();
}
