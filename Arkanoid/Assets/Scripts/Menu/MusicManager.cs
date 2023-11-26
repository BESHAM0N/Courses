using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public Slider musicSlider;
    public AudioSource musicAudio;
   
    private static readonly string FirstPlay = "FirstPlay";
    private static readonly string MusicPref = "MusicPref";
    private int _firstPlayInt;
    private float musicFloat;

    private void Start()
    {
        _firstPlayInt = PlayerPrefs.GetInt(FirstPlay);

        if (_firstPlayInt == 0)
        {
            musicFloat = 0.25f;
            musicSlider.value = musicFloat;
            PlayerPrefs.SetFloat(MusicPref, musicFloat);
            PlayerPrefs.SetInt(FirstPlay, -1);
        }
        else
        {
            musicFloat = PlayerPrefs.GetFloat(MusicPref);
            musicSlider.value = musicFloat;
        }
    }

    public void SaveSoundSettings()
    {
        PlayerPrefs.SetFloat(MusicPref, musicSlider.value);
    }

    private void OnApplicationFocus(bool inFocus)
    {
        if (!inFocus)
            SaveSoundSettings();
    }

    public void UpdateSound()
    {
        musicAudio.volume = musicSlider.value;         
    }

    public void StopMusic()
    {
        musicAudio.volume = 0;
        musicSlider.value = 0;
    }
    
}
