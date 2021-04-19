using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewGame : MonoBehaviour
{
    private int initialLifes = 3;

    // Start is called before the first frame update
    void Start()
    {
        DataManager.dataManager.lifes = initialLifes;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
            SceneManager.LoadScene("CutScene");
    }
}
