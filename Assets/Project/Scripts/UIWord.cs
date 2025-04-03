using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIWord : MonoBehaviour
{
    [SerializeField] private List<UIWordEmptyCluster> emptyClusters;
    public static int a = 0;
    public void Initialize(string[] clusters)
    {
        emptyClusters.ForEach(a => a.gameObject.SetActive(false));
        for (int i = 0; i < clusters.Length; i++)
        {
            var cluster = emptyClusters[i];
            cluster.Initialize(a + i);
        }
        a += clusters.Length;
    }

    public void Clear()
    {
        foreach (var emptyCluster in emptyClusters)
        {
            emptyCluster.Clear();
        }
    }

    void OnDestroy()
    {
        a = 0;
    }

    public IReadOnlyCollection<UIWordEmptyCluster> ActiveEmptyClusters
        => emptyClusters.Where(a => a.gameObject.activeSelf).ToList();
}
