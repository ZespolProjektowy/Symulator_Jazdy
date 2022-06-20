using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown resolutionDropdown;
    Resolution[] resolutions;
    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions(); 
        List<string> options  = new List<string>();
        int curr = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width
                && resolutions[i].height == Screen.currentResolution.height)
            {
                curr = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = curr;
        resolutionDropdown.RefreshShownValue();
    }
    public void setVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }
    public void setFullscreen(bool isFullScreen)
    { 
        Screen.fullScreen = isFullScreen; 
    }
}
