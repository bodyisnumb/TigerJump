using UnityEngine;
using UnityEngine.UI;

public class AudioMuter : MonoBehaviour
{
    public AudioSource audioSource;
    public Toggle muteToggle;

    private void Start()
    {
        if (audioSource != null && muteToggle != null)
        {
            muteToggle.isOn = audioSource.mute;
            muteToggle.onValueChanged.AddListener(OnToggleValueChanged);
        }
    }

    private void OnToggleValueChanged(bool isMuted)
    {
        if (audioSource != null)
        {
            audioSource.mute = isMuted;
            muteToggle.isOn = audioSource.mute;
        }
    }
}
