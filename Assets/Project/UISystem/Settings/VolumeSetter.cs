using UnityEngine;
namespace Settings
{
    public class VolumeSetter
    {
        public void SetVolumeValue(float value)
        {
            AudioListener.volume = value;
        }
    }
}