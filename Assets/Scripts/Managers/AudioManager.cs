using UnityEngine;

public class AudioManager : MonoBehaviour
{   
    // Audios that will be used
    private AudioSource backgroundAudio;
    private AudioSource gunAudio;
    private AudioSource playerhitAudio;
    private AudioSource fallAudio;
    private AudioSource powerupAudio;

    private static AudioManager inst = null;

    public static AudioManager GetAudioManager()
    {
        return inst;
    }

    public void Awake()
    {
        inst = this;
    }

    // Use this for initialization
    void Start()
    {
        AudioSource[] audioList = GetComponents<AudioSource>();              // Making a List and the Settings for the Audios

        backgroundAudio = audioList[0];
        backgroundAudio.loop = true;
        backgroundAudio.volume = 0.2f;

        gunAudio = audioList[1];
        gunAudio.loop = false;
        gunAudio.volume = 0.1f;

        playerhitAudio = audioList[2];
        playerhitAudio.loop = false;
        playerhitAudio.volume = 0.09f;

        fallAudio = audioList[3];
        fallAudio.loop = false;
        fallAudio.volume = 0.35f;

        powerupAudio = audioList[4];
        powerupAudio.loop = false;
        powerupAudio.volume = 0.4f;

        BackgroundAudio_ON();
    }

    // The functions of the Audios
    public void BackgroundAudio_ON()
    {
        backgroundAudio.Play();
    }

    public void GunAudio_ON()
    {
        gunAudio.Play();
    }

    public void PlayerhitAudio_ON()
    {
        playerhitAudio.Play();
    }

    public void FallAudio_ON()
    {
        fallAudio.Play();
    }

    public void PowerupAudio_ON()
    {
        powerupAudio.Play();
    }
}