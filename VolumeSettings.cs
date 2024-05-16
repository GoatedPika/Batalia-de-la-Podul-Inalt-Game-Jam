using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    public Slider volumeSlider;
    public AudioMixer mixer;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("master"))
        {
            PlayerPrefs.SetFloat("master", 1);
            Load();
        }
        else
            Load();
    }

    public void ChangeVolume()
    {
        mixer.SetFloat("master", Mathf.Log10(volumeSlider.value) * 20);
        Save();
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("master");
    }

    private void Save()
    {
        PlayerPrefs.SetFloat("master", volumeSlider.value);
    }


    
}
