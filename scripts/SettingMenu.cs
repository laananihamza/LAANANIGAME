using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Dropdown ResolutionDropdown;
    Resolution[] ResolutionsValue;
    void Start()
    {
        // push all Screen resolutions to Resolution Array
        ResolutionsValue = Screen.resolutions; // Returns all full-screen resolutions that the monitor supports
        // Declare Array Options
        List<string> Options = new List<string>(); // new Empty List

        int currentResoluationIndex = 0;
        for (int i = 0; i < ResolutionsValue.Length; i++)
        {
            // width * height
            string Option = ResolutionsValue[i].width + " × " + ResolutionsValue[i].height;
            Options.Add(Option);
            // if The chosen ResolutionDropdown index = to seem currentResolution we put that Resolution value on Label Of Dropdown 
            if (ResolutionsValue[i].width == Screen.currentResolution.width && ResolutionsValue[i].height == Screen.currentResolution.height)
            {
                currentResoluationIndex = i;
            }
        }

        ResolutionDropdown.ClearOptions();
        ResolutionDropdown.AddOptions(Options);
        ResolutionDropdown.value = currentResoluationIndex;
        ResolutionDropdown.RefreshShownValue();
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution Res = ResolutionsValue[resolutionIndex];
        Screen.SetResolution(Res.width, Res.height, Screen.fullScreen);
    }
    public void volume(float value)
    {
        // Volume Controller
        audioMixer.SetFloat("Volume", value);
    }

    public void SetWindows(bool isWindow)
    {
        Screen.fullScreen = isWindow;

    }

    public void Quality(int qualityChosen)
    {
        // The qualityChosen Param is for index of levels exmple => Low -> 0..
        QualitySettings.SetQualityLevel(qualityChosen);

    }
}