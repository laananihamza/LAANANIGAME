using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Statistics : MonoBehaviour
{
    [SerializeField] private Text TotalScore;
    [SerializeField] private Text HighScore;
    [SerializeField] private Text TotalDeath;

    // PlayerPrefs => class that stores Player preferences between game sessions.
    void Start()
    {
        Score.totalScore -= DeathTime.TotalDeath * 10;
        if (Score.totalScore < 0)
        {
            Score.totalScore = 0;
        }
        TotalScore.text = Score.totalScore.ToString();
        TotalDeath.text = DeathTime.TotalDeath.ToString();
        // Get the High Score, if doesn't exists set defult value (0)
        HighScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();

        // if the current Score great then High Score
        if (Score.totalScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            // will set the default value of HighScore to current Score
            PlayerPrefs.SetInt("HighScore", Score.totalScore);
            HighScore.text = Score.totalScore.ToString();
        }
    }

    public void NextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
