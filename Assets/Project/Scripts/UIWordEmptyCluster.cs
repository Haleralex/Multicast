using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIWordEmptyCluster : MonoBehaviour
{
    [HideInInspector] public int Index { get; private set; }

    public RectTransform rectTransform => GetComponent<RectTransform>();

    public bool IsOccupied { get; private set; }

    public void Initialize(int index)
    {
        Index = index;
        SetOccupied(false);
        gameObject.SetActive(true);
    }

    public void Clear()
    {
        if(transform.childCount == 0) return;
        if(transform.GetChild(0)!=null){
            Destroy(transform.GetChild(0).gameObject);
        }
    }

    public void SetOccupied(bool occupied)
    {
        IsOccupied = occupied;
    }
}
