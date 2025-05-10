using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Core
{
    [CreateAssetMenu(fileName = "AudioSettingsConfig", menuName = "ScriptableObjects/AudioSettingsConfig")]
    public class AudioSettingsConfig : ScriptableObject
    {
        [SerializeField, SerializedDictionary("Audio Type", "Audio Clip")]
        private SerializedDictionary<AudioType, AudioClip> audioClips = new();
        public IReadOnlyDictionary<AudioType, AudioClip> AudioClips => audioClips;
    }
}