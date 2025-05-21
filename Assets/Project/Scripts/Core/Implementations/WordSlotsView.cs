using System.Collections.Generic;
using ZLinq;
using UnityEngine;

public class WordSlotsView : MonoBehaviour
{
    private Dictionary<int, WordPieceSlot> wordPieceSlots = new();

    public void SetWordPieceSlots(List<WordPieceSlot> slots)
    {
        wordPieceSlots.Clear();

        foreach (var slot in slots)
        {
            wordPieceSlots[slot.Index] = slot;
        }
    }

    public WordPieceSlot FindClosestEmptySlot(Vector3 position, float maxDistance = 10f)
    {
        var notOccupiedSlots = wordPieceSlots.Values
            .AsValueEnumerable().Where(slot => !slot.IsOccupied).ToList();
        var orderedByDistanceSlots = notOccupiedSlots
            .AsValueEnumerable().OrderBy(slot => Vector2.Distance(slot.rectTransform.position, position))
            .ToList();
        var fitSlot = orderedByDistanceSlots
                        .AsValueEnumerable().FirstOrDefault(slot => Vector2.Distance(slot.rectTransform.position, position) < maxDistance);

        return fitSlot;
    }

    public void ResetAllSlots()
    {
        foreach (var slot in wordPieceSlots.Values)
        {
            slot.SetOccupied(false);
        }
    }

    public bool TryGetSlot(int index, out WordPieceSlot slot)
    {
        return wordPieceSlots.TryGetValue(index, out slot);
    }

    public void UpdateSlotsFromMappings(IReadOnlyDictionary<int, int> mappings)
    {

        foreach (var slot in wordPieceSlots.Values)
        {
            slot.SetOccupied(false);
        }
        foreach (var mapping in mappings)
        {
            if (wordPieceSlots.TryGetValue(mapping.Value, out var slot))
            {
                slot.SetOccupied(true);
            }
        }
    }
}