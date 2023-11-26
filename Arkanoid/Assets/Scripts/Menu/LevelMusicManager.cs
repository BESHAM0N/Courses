using UnityEngine;

public class LevelMusicManager : MonoBehaviour
{
    public AudioSource musicAudio;
    private float musicFloat;
    private static readonly string MusicPref = "MusicPref";

    private void Awake()
    {
        LevelSoundsSettings();
    }

    private void LevelSoundsSettings()
    {
        musicFloat = PlayerPrefs.GetFloat(MusicPref);
        musicAudio.volume = musicFloat;
    }
}
