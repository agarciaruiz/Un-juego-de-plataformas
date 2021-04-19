using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public AudioSource gameOverSource;
    public AudioClip gameOver;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        gameOverSource.PlayOneShot(gameOver);
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene("StartGame");
    }
}
