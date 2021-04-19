using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private float bounceHeight = 0.5f;
    private float bounceSpeed = 4.0f;

    private Vector2 originalPos;

    private void Start()
    {
        originalPos = transform.localPosition;
    }

    public void BlockBounce()
    {
        StartCoroutine(Bounce());
    }

    IEnumerator Bounce()
    {
        while (true)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + bounceSpeed * Time.deltaTime);
            if (transform.localPosition.y >= originalPos.y + bounceHeight)
                break;

            yield return null;
        }

        while (true)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - bounceSpeed * Time.deltaTime);
            if (transform.localPosition.y <= originalPos.y)
            {
                transform.localPosition = originalPos;
                break;
            }


            yield return null;
        }
    }
}

