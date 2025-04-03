using UnityEngine;

public class UIClusterFactory : MonoBehaviour
{
    [SerializeField] private UIClusterElement clusterElementPrefab;
    public UIClusterElement CreateClusterElement()
    {
        var clusterElement = Instantiate(clusterElementPrefab, transform);
        return clusterElement;
    }
}
