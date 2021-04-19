using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerUnit : MonoBehaviour
{
    public int currentLifes;
    public static PlayerUnit playerUnit;

    public AudioSource bsoAudio;
    public AudioSource playerFX;
    public AudioClip jumpSound;
    public AudioClip coinSound;
    public AudioClip mushroomShowup;
    public AudioClip powerUp;
    public AudioClip bounceSound;
    public AudioClip breakSound;
    public AudioClip marioDies;
    public AudioClip levelCompleted;

    private void Start()
    {
        playerUnit = this;
        currentLifes = PlayerPrefs.GetInt("lifes");
    }

    private void Update()
    {
        if (gameObject.transform.position.y < -30)
        {
            currentLifes -= 1;
            StartCoroutine(Died());
        }
    }

    public IEnumerator Died()
    {
        DataManager.dataManager.lifes = PlayerUnit.playerUnit.currentLifes;
        DataManager.dataManager.SaveData();
        GetComponent<Animator>().Play("Die");
        GetComponent<BoxCollider2D>().enabled = false;

        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(3);

        if (PlayerUnit.playerUnit.currentLifes != 0)
        {
            SceneManager.LoadScene("CutScene");
        }
        else
            SceneManager.LoadScene("GameOver");
        Time.timeScale = 1;
    }
}
