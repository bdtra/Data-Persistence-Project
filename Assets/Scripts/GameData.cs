using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using TMPro;

public class GameData : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameData instance;
    public int highScore;
    public string highScorePlayerName;
    public string playerName;
    

    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        LoadGameData();
    }

    [System.Serializable]
    class SaveData
    {
        public int highScore;
        public string highScorePlayerName;
    }

    public void SaveGame(int points, string name)
    {
        //if the current game's points exceed the highscore, save the player name and score to the save file
        if (points > highScore)
        {
            SaveData data = new SaveData();

            data.highScore = points;
            data.highScorePlayerName = name;

            string json = JsonUtility.ToJson(data);
            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }
    }

    public void LoadGameData()
    {

        //File pathing
        string path = Application.persistentDataPath + "/savefile.json";

        //if the file exists, read the json and set this instance's variables to its data.
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            highScore = data.highScore;
            highScorePlayerName = data.highScorePlayerName;
        }
    }
}
