using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;

    public AudioMixer mixer;

    [Header("Sources")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("Music")]
    public AudioClip[] tracks;

    private int currentTrack = 0;
    public int CurrentTrackIndex => currentTrack;

    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start() {
        float master = PlayerPrefs.GetFloat("MASTER", 1f);
        float bgm = PlayerPrefs.GetFloat("BGM", 1f);
        float sfx = PlayerPrefs.GetFloat("SFX", 1f);

        SetMasterVolume(master);
        SetBGMVolume(bgm);
        SetSFXVolume(sfx);

        PlayBGM(0);
    }

    public void PlayBGM(int index) {
        if (index < 0 || index >= tracks.Length) return;

        currentTrack = index;

        bgmSource.clip = tracks[index];
        bgmSource.Play();
    }

    public void PlaySFX(AudioClip clip) {
        sfxSource.PlayOneShot(clip);
    }

    public void SetBGMVolume(float value) {
        mixer.SetFloat("BGMVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("BGM", value);
    }

    public void SetSFXVolume(float value) {
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SFX", value);
    }

    public void SetMasterVolume(float value) {
        mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("MASTER", value);
    }
}