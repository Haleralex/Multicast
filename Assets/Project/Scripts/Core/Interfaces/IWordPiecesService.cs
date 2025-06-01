using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IWordPiecesService
{
    UniTask<List<WordPiece>> CreateWordPieces(
        Dictionary<int, Dictionary<int, string>> correctMapping,
        Dictionary<int, string> missingSlots);
    UniTask ResetWordPieces();
}