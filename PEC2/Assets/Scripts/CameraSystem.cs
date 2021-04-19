using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    private GameObject player;
    private float xMin = 2.5f;
    private float xMax = 215.0f;
    private float yMin = 1.5f;
    private float yMax = 7.0f;
    private Vector3 offset;
    private Vector3 cameraPos;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //cameraPos = transform.position;
        offset = transform.position - player.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = player.transform.position + offset;

        float xBoundaries = Mathf.Clamp(player.transform.position.x, xMin, xMax);
        float yBoundaries = Mathf.Clamp(player.transform.position.y, yMin, yMax);
        transform.position = new Vector3(xBoundaries, yBoundaries, transform.position.z);
    }
}
