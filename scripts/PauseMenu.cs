using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    bool PausedGame = false;
    public GameObject PauseMenuPanel;
    public GameObject OptionMenuPanel;
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

    void Update()
    {
        // check if player press the Esc key
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (PausedGame)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution Res = ResolutionsValue[resolutionIndex];
        Screen.SetResolution(Res.width, Res.height, Screen.fullScreen);
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

    public void ResumeGame()
    {
        // For slow motion effect. // To stop = 0 / run = 1 Time of levels
        Time.timeScale = 1f;
        // To Disable The PauseMenuPanel (canvas -> Panel).
        PauseMenuPanel.SetActive(false);
        OptionMenuPanel.SetActive(false);
        PausedGame = false;
    }
    void PauseGame()
    {

        Time.timeScale = 0f;
        PauseMenuPanel.SetActive(true);
        PausedGame = true;
    }
    /* Optional if want to add restart Game */
    // public void Restart()
    // {
    //     Time.timeScale = 1f;
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    // }
    public void QuitToMenu()
    {

        SceneManager.LoadScene(0);
    }
}
