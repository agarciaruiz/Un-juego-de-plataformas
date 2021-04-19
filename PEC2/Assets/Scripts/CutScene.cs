using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutScene : ScoreSystem
{
    public GameObject lifesText;
    public GameObject highScoreText;

    // Start is called before the first frame update
    void Start()
    {
        currentLifes = DataManager.dataManager.lifes;
        playerScore = DataManager.dataManager.actualScore;
        DataManager.dataManager.actualScore = 0;

        if (playerScore > highScore) {
            highScore = playerScore;
            DataManager.dataManager.SaveData();
            DataManager.dataManager.highScore = highScore;
        }

        PlayerPrefs.SetInt("lifes", currentLifes);

        lifesText.gameObject.GetComponent<Text>().text = "X " + currentLifes;
        scoreText.gameObject.GetComponent<Text>().text = "Score: " + playerScore;
        highScoreText.gameObject.GetComponent<Text>().text = "Top x " + DataManager.dataManager.highScore;


        DataManager.dataManager.SaveData();
        StartCoroutine(Restart());

    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("GameScene");
      
    }
}
