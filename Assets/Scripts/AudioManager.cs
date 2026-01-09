using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource MusicSource;
    public AudioClip Hit;
    public AudioClip Whistle;
    public AudioClip Ambiance;
    public AudioClip Grynt1;
    public AudioClip Grynt2;
    public AudioClip Sand1;
    public AudioClip Sand2;
    public AudioClip Kids;
    public AudioClip Finish;
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
