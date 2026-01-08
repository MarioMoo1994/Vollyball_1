using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource MusicSource;
    public AudioClip Hit;
    public AudioClip Whistle;
    public AudioClip Ambiance;

        private void Start()
    {
        MusicSource.clip = Ambiance;
        MusicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
