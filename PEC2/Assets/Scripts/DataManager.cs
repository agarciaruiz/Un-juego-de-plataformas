using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DataManager : MonoBehaviour
{
    public static DataManager dataManager;

    public int highScore;
    public int actualScore;
    public int lifes = 3;

    private void Awake()
    {
        if (dataManager == null)
        {
            DontDestroyOnLoad(gameObject);
            dataManager = this;
        } else if (dataManager != this)
            Destroy(gameObject);
    }

    public void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/gameInfo.dat");
        gameData data = new gameData();
        data.highscore = highScore;
        data.actualScore = actualScore;
        data.lifes = lifes;
        bf.Serialize(file, data);
        file.Close();
    }

    public void LoadData()
    {
        if(File.Exists(Application.persistentDataPath + "/gameInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/gameInfo.dat", FileMode.Open);
            gameData data = (gameData)bf.Deserialize(file);
            file.Close();
            highScore = data.highscore;
            actualScore = data.actualScore;
            lifes = data.lifes;
        }
    }
}

[Serializable]
class gameData
{
    public int highscore;
    public int actualScore;
    public int lifes;
}