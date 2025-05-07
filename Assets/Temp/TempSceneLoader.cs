using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

public class TempSceneLoader : MonoBehaviour
{
    void Start()
    {
        Addressables.LoadSceneAsync("Scene2", LoadSceneMode.Single).Completed += handle =>
            {
                if (handle.Status == AsyncOperationStatus.Succeeded)
                {
                    Debug.Log("Gameplay scene loaded successfully.");
                }
                else
                {
                    Debug.LogError("Failed to load gameplay scene.");
                }
            };
    }
}
