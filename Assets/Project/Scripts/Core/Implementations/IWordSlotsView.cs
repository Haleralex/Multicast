using System.Collections.Generic;
using UnityEngine;

public interface IWordSlotsView
{
    void SetWordPieceSlots(List<WordPieceSlot> slots);
    WordPieceSlot FindClosestEmptySlot(Vector3 position, float maxDistance = 10f);
    void ResetAllSlots();
    bool TryGetSlot(int index, out WordPieceSlot slot);
    void SetOccupationCondition(MappingUpdate mappings, IReadOnlyDictionary<int, int> wordPieceMappings = null);
}
