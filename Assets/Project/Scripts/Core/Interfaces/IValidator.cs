using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IValidator
{
    bool IsLevelValid(string expectedWord, string actualWord, bool caseSensitive = false);
    bool IsLevelValid(string expectedWord, Dictionary<int, string> wordPiecesFragments, IReadOnlyDictionary<int, int> mappings, bool caseSensitive = false);
}
