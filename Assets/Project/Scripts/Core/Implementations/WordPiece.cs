using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WordPiece : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public event Action<WordPiece> Selected;
    public event Action<WordPiece> Released;
    public event Action<WordPiece> DoubleClicked;
    [SerializeField] private TMPro.TextMeshProUGUI fragmentText;
    private const float MIN_DISTANCE_TO_DRAG = 10;
    private string fragment;
    public string Fragment => fragment;
    private float lastClickTime;
    private float doubleClickTimeThreshold = 0.3f;
    private bool isPointerDown;
    private Vector2 cachedEventDataPosition;
    public RectTransform rectTransform;
    public int Index;
    public void Initialize(string fragment, int index)
    {
        this.fragment = fragment;
        fragmentText.text = this.fragment;

        Index = index;
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        cachedEventDataPosition = eventData.position;

        float timeSinceLastClick = Time.time - lastClickTime;
        if (timeSinceLastClick < doubleClickTimeThreshold)
        {
            DoubleClicked?.Invoke(this);
        }

        lastClickTime = Time.time;
    }
    public void ChangeParent(Transform newParent, bool nullifyPosition = false)
    {
        transform.SetParent(newParent, true);

        if (nullifyPosition)
            rectTransform.localPosition = Vector3.zero;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPointerDown)
            return;

        isPointerDown = false;

        Released?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var magnitude = (eventData.position - cachedEventDataPosition).magnitude;
        if (magnitude > MIN_DISTANCE_TO_DRAG && !isPointerDown)
        {
            isPointerDown = true;
            Selected?.Invoke(this);
        }
    }
}
