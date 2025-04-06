using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Data
{

    /// <summary>
    /// Represents the data structure for a word in the game, including its clusters and missing clusters.
    /// </summary>
    [Serializable]
    public class WordData
    {
        [JsonProperty("clusters")]
        public List<string> Clusters { get; set; }

        [JsonProperty("missingClusters")]
        public List<int> MissingClusters { get; set; }

        public WordData()
        {
            Clusters = new List<string>();
            MissingClusters = new List<int>();
        }
    }
}