using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// Represents the data structure for a level in the game.
    /// </summary>

    [Serializable]
    public class LevelData
    {
        [JsonProperty("Words")]
        public List<WordData> Words { get; set; }

        public LevelData()
        {
            Words = new List<WordData>();
        }
    }

}