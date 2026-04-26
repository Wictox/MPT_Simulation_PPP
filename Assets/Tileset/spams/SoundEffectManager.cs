using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    public static SoundEffectManager Instance;

    private static AudioSource audioSource;
    private static AudioSource voiceAudioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    
    [SerializeField] private Slider sfxSlider;

    public void Awake()
    {
        // FIX 3: Changed to == null so it doesn't destroy itself on startup!
        if (Instance == null)
        {
            Instance = this;
            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSource = audioSources[0]; // Assuming the first audio source is for sound effects
            voiceAudioSource = audioSources[1]; // Assuming the voice audio source is the second one
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play (string soundName)
    {
        // FIX 1: Changed to lowercase 's' to use your actual variable
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    public static void PlayVoice(AudioClip audioClip , float pitch = 1f)
    {
        voiceAudioSource.pitch = pitch;
        voiceAudioSource.PlayOneShot(audioClip);
    }

    void Start()
    {
        // FIX 2: Simplified the listener so it passes the float correctly
        sfxSlider.onValueChanged.AddListener(OnValueChanged);
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
        voiceAudioSource.volume = volume;
    }

    public void OnValueChanged(float value)
    {
        // Passed the value directly to the SetVolume function
        SetVolume(value); 
    }
}