using System.Runtime.InteropServices;
using Core;
using UnityEngine;
using Zenject;

public class ServicesStarter : IServicesStarter
{
    private IAudioManager _audioManager;

    [Inject]
    public ServicesStarter(IAudioManager audioManager)
    {
        _audioManager = audioManager;

        SetupMusicService();
    }

    public void SetupMusicService()
    {
        _audioManager.PlayBackgroundMusic();
    }
}
