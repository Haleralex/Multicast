using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IAudioLoader
{
    UniTask<AudioClip> LoadAudioClipAsync(string address);
    void ReleaseAudioClip(string address);
}
