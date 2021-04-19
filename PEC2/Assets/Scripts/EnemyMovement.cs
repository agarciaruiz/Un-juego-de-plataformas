using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float enemySpeed = 2f;
    private int enemyScore = 100;
    Rigidbody2D rb;
    SpriteRenderer renderer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        renderer = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        if (renderer.isVisible)
        {
            rb.velocity = new Vector2 (enemySpeed, rb.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag != "Ground" && collision.gameObject.tag != "Brick")
            enemySpeed *= -1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Feet")
        {
            this.enabled = false;
            GetComponent<Animator>().Play("EnemyDies");
            gameObject.tag = "Untagged";
            Destroy(gameObject, 0.5f);
            ScoreSystem.score.playerScore += enemyScore;
            DataManager.dataManager.highScore = ScoreSystem.score.playerScore;
        }
    }
}
