using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Validator : IValidator
{
    public bool IsLevelValid(string expectedWord, string actualWord, bool caseSensitive = false)
    {
        if (string.IsNullOrEmpty(expectedWord) || string.IsNullOrEmpty(actualWord))
        {
            return false;
        }

        return string.Compare(
            expectedWord,
            actualWord,
            !caseSensitive,
            System.Globalization.CultureInfo.CurrentCulture) == 0;
    }

    public bool IsLevelValid(string expectedWord, Dictionary<int, string> wordPiecesFragments,
        IReadOnlyDictionary<int, int> mappings, bool caseSensitive = false)
    {
        if (mappings == null || mappings.Count == 0 || wordPiecesFragments == null)
        {
            return false;
        }

        string actualWord = BuildWordFromMappings(wordPiecesFragments, mappings);

        return IsLevelValid(expectedWord, actualWord, caseSensitive);
    }

    private string BuildWordFromMappings(Dictionary<int, string> fragments, IReadOnlyDictionary<int, int> mappings)
    {
        var sortedMappings = new List<KeyValuePair<int, int>>(mappings);
        sortedMappings.Sort((a, b) => a.Value.CompareTo(b.Value));

        string result = string.Empty;
        foreach (var mapping in sortedMappings)
        {
            int wordPieceId = mapping.Key;
            if (fragments.TryGetValue(wordPieceId, out string fragment))
            {
                result += fragment;
            }
        }

        return result;
    }
}
