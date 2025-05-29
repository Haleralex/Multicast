using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Core
{
    public class AudioLoader : IAudioLoader
    {
        private readonly Dictionary<string, AsyncOperationHandle<AudioClip>> _loadedClips = new();

        public async UniTask<AudioClip> LoadAudioClipAsync(string address)
        {
            if (_loadedClips.TryGetValue(address, out var cachedHandle))
            {
                if (cachedHandle.IsValid() && cachedHandle.Status == AsyncOperationStatus.Succeeded)
                {
                    return cachedHandle.Result;
                }
            }

            var handle = Addressables.LoadAssetAsync<AudioClip>(address);
            _loadedClips[address] = handle;
            
            await handle.ToUniTask();
            var audioClip = handle.Result;
            if (audioClip == null)
            {
                Debug.LogError($"Failed to load audio clip at address: {address}");
                _loadedClips.Remove(address);
            }
            
            return audioClip;
        }

        public void ReleaseAudioClip(string address)
        {
            if (_loadedClips.TryGetValue(address, out var handle))
            {
                if (handle.IsValid())
                {
                    Addressables.Release(handle);
                }
                _loadedClips.Remove(address);
            }
        }

        public void ReleaseAll()
        {
            foreach (var kvp in _loadedClips)
            {
                if (kvp.Value.IsValid())
                {
                    Addressables.Release(kvp.Value);
                }
            }
            _loadedClips.Clear();
        }

        ~AudioLoader()
        {
            ReleaseAll();
        }
    }
}