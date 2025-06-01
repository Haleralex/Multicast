using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;



public interface IWordPieceSlotsService
{
    UniTask<List<WordPieceSlot>> CreateWordPieceSlots(
        Dictionary<int, Dictionary<int, string>> correctMapping,
        Dictionary<int, string> missingSlots);
    UniTask ResetSlots();
}