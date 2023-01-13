// IO => we want to work with files on operator system
using System.IO;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveDataSystem
{
    public static void SavePlayerData(player pl)
    {

        // Path where we create file
        string mySavepath = Application.persistentDataPath + "/tlbunny.json";

        SaveData SData = new SaveData(pl);
        // To convert data to json file
        string json = JsonUtility.ToJson(SData);

        // To create file on our system
        using StreamWriter writer = new StreamWriter(mySavepath);

        writer.Write(json);
    }
    public static SaveData LoadPlayerData()
    {

        string mySavePath = Application.persistentDataPath + "/tlbunny.json";
        // we see if file exists or not 
        if (File.Exists(mySavePath))
        {
            // if file exists will reader its content
            StreamReader reader = new StreamReader(mySavePath);

            // Read content of file
            string json = reader.ReadToEnd();

            // Then convert json to string
            SaveData SData = JsonUtility.FromJson<SaveData>(json);
            // After that we return the content value 
            return SData;
        }
        else
        {
            // if file doesn't exists we sent null
            return null;
        }


    }

}
