using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// that mean we will save it in a file
[System.Serializable]
public class SaveData
{
    public int Tscore;
    public int level;
    public int death;

    public SaveData(player pl)
    {

        level = Levels.Level;
        Tscore = Score.totalScore;
        death = DeathTime.TotalDeath;
    }
}
