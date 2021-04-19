using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomMovement : MonoBehaviour
{
    public float mushroomSpeed = 2f;
    private Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GetComponent<Animator>().enabled = false;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(mushroomSpeed, rb.velocity.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag != "Ground" && collision.gameObject.tag != "Brick" && collision.gameObject.tag != "Lootbox")
            mushroomSpeed *= -1;
    }
}
