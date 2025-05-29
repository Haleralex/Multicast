using UnityEngine;
namespace Settings
{
    public class VolumeSetter : IVolumeSetter
    {
        public void SetVolume(float value)
        {
            AudioListener.volume = value;
        }
    }
}