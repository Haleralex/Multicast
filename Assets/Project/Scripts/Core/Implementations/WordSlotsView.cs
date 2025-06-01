using System.Collections.Generic;
using ZLinq;
using UnityEngine;
using System.Linq;

/// <summary>
/// Service for managing the slots where word pieces can be placed.
/// </summary>
public class WordSlotsView : IWordSlotsView
{
    private readonly Dictionary<int, WordPieceSlot> wordPieceSlots = new();

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
            .AsValueEnumerable()
            .Where(slot => !slot.IsOccupied)
            .ToList();
        var orderedByDistanceSlots = notOccupiedSlots
            .AsValueEnumerable()
            .OrderBy(slot => Vector2.Distance(slot.rectTransform.position, position))
            .ToList();
        var fitSlot = orderedByDistanceSlots
            .AsValueEnumerable()
            .FirstOrDefault(slot =>
                Vector2.Distance(slot.rectTransform.position, position) < maxDistance);

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

    public void SetOccupationCondition(MappingUpdate mappings, IReadOnlyDictionary<int, int> wordPieceMappings = null)
    {
        switch (mappings.Type)
        {
            case MappingUpdate.UpdateType.Add:
                if (mappings.SlotId.HasValue)
                {
                    var addedSlot = wordPieceSlots[mappings.SlotId.Value];
                    addedSlot.SetOccupied(true);
                }
                break;

            case MappingUpdate.UpdateType.Remove:
                if (mappings.SlotId.HasValue)
                {
                    var removedSlot = wordPieceSlots[mappings.SlotId.Value];
                    removedSlot.SetOccupied(false);
                }
                break;
            case MappingUpdate.UpdateType.InitializeAll:
                foreach (var slotTuple in wordPieceMappings)
                {
                    var slot = wordPieceSlots[slotTuple.Value];
                    slot.SetOccupied(true);
                }
                break;
        }
    }
}