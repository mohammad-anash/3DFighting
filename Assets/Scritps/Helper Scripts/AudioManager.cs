using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource sfxSource;

    private void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        sfxSource = gameObject.AddComponent<AudioSource>();

    }

    public void PlaySFX(AudioClip clip)
    {
        if (clip == null) return;

        sfxSource.PlayOneShot(clip);
    }
}
