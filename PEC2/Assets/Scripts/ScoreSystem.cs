using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScoreSystem : PlayerUnit
{
    private float timeLeft = 120.0f;
    public int playerScore = 0;
    public int highScore;
    public GameObject timerText;
    public GameObject scoreText;
    public GameObject Player;
    public static ScoreSystem score;

    private void Start()
    {
        score = this;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;

        timerText.gameObject.GetComponent<Text>().text = "Time left: " + (int)timeLeft;
        scoreText.gameObject.GetComponent<Text>().text = "Score: " + playerScore;

        if (timeLeft < 0.1f)
            StartCoroutine(Died());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "FinishFlag")
        {
            TimeScoreSum();
            StartCoroutine(FinishLevel());
        }
    }

    void TimeScoreSum()
    {
        playerScore += (int)(timeLeft * 10);
        DataManager.dataManager.actualScore = playerScore;
        DataManager.dataManager.SaveData();
    }

    IEnumerator FinishLevel()
    {
        Player.GetComponent<Animator>().Play("Finish");
        PlayerUnit.playerUnit.bsoAudio.Stop();
        PlayerUnit.playerUnit.playerFX.PlayOneShot(PlayerUnit.playerUnit.levelCompleted);
        yield return new WaitForSeconds(6);
        SceneManager.LoadScene("GameOver");
    }

}
