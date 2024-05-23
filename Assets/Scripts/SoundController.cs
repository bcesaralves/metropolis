using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance;

    public AudioSource effectsSource;

    public List<SoundEffect> soundEffects;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySoundEffect(string name)
    {
        SoundEffect soundEffect = soundEffects.Find(se => se.name == name);
        if (soundEffect != null)
        {
            effectsSource.PlayOneShot(soundEffect.clip);
        }
        else
        {
            Debug.LogWarning("Sound effect not found: " + name);
        }

    }

    public void SetEffectsVolume(float volume)
    {
        effectsSource.volume = volume;
    }
}
