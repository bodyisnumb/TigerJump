using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{



    public AudioSource audioSource;
    public AudioSource audioSourceBush;
    public AudioClip button;
    public AudioClip battery_recharging;
    public AudioClip bush;
    public AudioClip electricity;
    public AudioClip shield_guard;
    public AudioClip success;




    public void ButtonSound()
    {
        audioSource.clip = button;
        audioSource.Play();
    }

    public void BatterySound()
    {
        audioSource.clip = battery_recharging;
        audioSource.Play();
    }

    public void BushSound()
    {
        audioSourceBush.clip = bush;
        audioSourceBush.Play();
    }

    public void BombSound()
    {
        audioSource.clip = shield_guard;
        audioSource.Play();
    }

    public void ShieldSound()
    {
        audioSource.clip = electricity;
        audioSource.Play();
    }

    public void SuccessSound()
    {
        audioSource.clip = success;
        audioSource.Play();
    }

}
