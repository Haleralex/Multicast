using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IAssetLoader
{
    public event Action LoadingStarted;
    public event Action LoadingCompleted;
    public event Action<float> ProgressChanged;
    public UniTask LoadAssets(int fakeLoadDelayMilliSeconds = 0);
}
