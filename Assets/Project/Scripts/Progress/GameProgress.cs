using System;
using System.Collections.Generic;
using UnityEngine;
namespace Progress
{
    /// <summary>
    /// This class is responsible for managing the game progress using PlayerPrefs.
    /// </summary>
    [Serializable]
    public class GameProgress
    {
        public int CurrentLevel;
        public Dictionary<int, int> WordPiecesMapping = new();

        public List<string> GuessedWords = new();

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            if (obj is not GameProgress other)
                return false;

            if (CurrentLevel != other.CurrentLevel)
                return false;

            if (WordPiecesMapping.Count != other.WordPiecesMapping.Count)
                return false;

            foreach (var kvp in WordPiecesMapping)
            {
                if (!other.WordPiecesMapping.TryGetValue(kvp.Key, out var value) || value != kvp.Value)
                    return false;
            }

            return true;
        }
    }
}