using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core
{
    [CreateAssetMenu(fileName = "AudioSettingsConfig", menuName = "ScriptableObjects/AudioSettingsConfig")]
    public class AudioSettingsConfig : ScriptableObject
    {
        [SerializeField, SerializedDictionary("Audio Type", "Audio Clip")]
        private SerializedDictionary<AudioType, AssetReferenceT<AudioClip>> audioClips = new();
        public IReadOnlyDictionary<AudioType, AssetReferenceT<AudioClip>> AudioClips => audioClips;
    }
}