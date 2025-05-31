using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class WordPieceSlot : MonoBehaviour
{
    [Inject] private readonly IWordPieceSlotAnimator wordPieceSlotAnimator;
    [HideInInspector] public int Index { get; private set; }
    public RectTransform rectTransform => GetComponent<RectTransform>();
    [SerializeField] private TMPro.TextMeshProUGUI value;
    [SerializeField] private Image borders;
    public bool IsOccupied;// { get; private set; }
    public Color FixedSlotColor = Color.green;
    public Color EmptySlotColor = Color.blue;

    public void Initialize(int index)
    {
        this.value.text = string.Empty;
        Index = index;
        SetOccupied(false);
        gameObject.SetActive(true);
        borders.color = EmptySlotColor;
    }

    public void Initialize(int index, string value)
    {
        Index = index;
        this.value.text = value;
        SetOccupied(true);
        gameObject.SetActive(true);
        borders.color = FixedSlotColor;
    }

    public void SetClosestSlotAniimation()
    {
        wordPieceSlotAnimator?.SetClosestSlotAnimation(this);
    }
    public void ResetToDefaultCondition()
    {
        wordPieceSlotAnimator?.ResetToDefaultCondition(this);
    }

    public void SetOccupied(bool occupied)
    {
        IsOccupied = occupied;
        ResetToDefaultCondition();
    }
}
