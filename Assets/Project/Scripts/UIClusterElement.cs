using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIClusterElement : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public event Action<UIClusterElement> ClusterSelected;
    public event Action<UIClusterElement> ClusterReleased;
    public event Action<UIClusterElement> ClusterDoubleClicked;
    [SerializeField] private TMPro.TextMeshProUGUI clusterText;
    private float MIN = 10;
    private string cluster;
    public string Cluster => cluster;
    private float lastClickTime;
    private float doubleClickTimeThreshold = 0.3f;
    private bool isPointerDown;
    private Vector2 cachedEventDataPosition;
    private Transform cachedTransform;
    public RectTransform rectTransform;

    public int Index;

    public void Initialize(string cluster, int index)
    {
        this.cluster = cluster;
        clusterText.text = this.cluster;
        cachedTransform = transform.parent;

        Index = index;
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        cachedEventDataPosition = eventData.position;

        float timeSinceLastClick = Time.time - lastClickTime;
        if (timeSinceLastClick < doubleClickTimeThreshold)
        {
            ClusterDoubleClicked?.Invoke(this);
        }

        lastClickTime = Time.time;
    }
    public void ChangeParent(Transform newParent)
    {
        rectTransform.SetParent(newParent);
    }
    public void Return()
    {
        rectTransform.transform.localPosition = Vector3.zero;
        rectTransform.SetParent(cachedTransform);
        rectTransform.SetSiblingIndex(Index);
        rectTransform.anchoredPosition = Vector2.zero;
        Canvas.ForceUpdateCanvases();
        LayoutRebuilder.ForceRebuildLayoutImmediate(cachedTransform.GetComponent<RectTransform>());
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isPointerDown)
            return;

        isPointerDown = false;

        ClusterReleased?.Invoke(this);
    }

    public void OnDrag(PointerEventData eventData)
    {
        var magnitude = (eventData.position - cachedEventDataPosition).magnitude;
        if (magnitude > MIN)
        {
            isPointerDown = true;
            ClusterSelected?.Invoke(this);
        }
    }
}
