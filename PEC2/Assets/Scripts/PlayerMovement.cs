using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class PlayerMovement : PlayerUnit
{
    internal bool extraLife;
    public GameObject particles;

    // Run params
    private float playerSpeed = 15f;
    private float maxSpeed = 10f;
    private float linearDrag = 4.0f;
    private Vector2 direction;

    // Jump params
    private float jumpPower = 15f;
    private float enemyKill = 100f;
    private float jumpDelay = 0.25f;
    private float jumpTimer;

    // Physics params
    private float fallPower = 10f;
    private float rayLenght = 0.7f;
    private float gravity = 1f;
    public bool isOnGround = false;
    public bool isOnEnemy = false;
    public bool isOnCeiling = false;

    // Components
    private Rigidbody2D rb;
    private GameObject feet;
    public LayerMask groundLayer;
    public LayerMask boxLayer;
    public LayerMask enemyLayer;
    private Vector3 colliderOffset;
    private int mushroomScore = 1000;

    // Score values
    private int coinScore = 100;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        colliderOffset.x = 0.25f;
    }

    // Update is called once per frame
    void Update()
    {
        bool wasOnGound = isOnGround;
        direction = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        RaycastHit2D hitDownRayLeft = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, rayLenght, groundLayer | boxLayer); 
        RaycastHit2D hitDownRayRight = Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, rayLenght, groundLayer | boxLayer);
        
        RaycastHit2D hitEnemyDownLeft = Physics2D.Raycast(transform.position + colliderOffset, Vector2.down, rayLenght, enemyLayer);
        RaycastHit2D hitEnemyDownRight = Physics2D.Raycast(transform.position - colliderOffset, Vector2.down, rayLenght, enemyLayer);

        isOnGround = hitDownRayLeft || hitDownRayRight;

        isOnEnemy = hitEnemyDownLeft || hitEnemyDownRight;

        if (!wasOnGound && isOnGround)
        {
            GetComponent<Animator>().SetBool("IsJumping", false);
        }

        // Jump
        if (Input.GetButtonDown ("Jump"))
        {
            jumpTimer = Time.time + jumpDelay;
        }
    }

    private void FixedUpdate()
    {
        PlayerMove(direction.x);

        if (jumpTimer > Time.time && isOnGround)
            Jump();

        ModifyPhysics();
        Raycasting();
    }

    void PlayerMove(float horizontal)
    {
        // Run 
        rb.AddForce(Vector2.right * horizontal * playerSpeed);
        GetComponent<Animator>().SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        // Velocity control
        if (Mathf.Abs(rb.velocity.x) > maxSpeed)
            rb.velocity = new Vector2 (Mathf.Sign(rb.velocity.x) * maxSpeed, rb.velocity.y);
        
        // Direction flip
        if (direction.x < 0.0f)
            GetComponent<SpriteRenderer>().flipX = true;
        else if (direction.x > 0.0f)
            GetComponent<SpriteRenderer>().flipX = false;
    }

    void Jump()
    {
        PlayerUnit.playerUnit.playerFX.PlayOneShot(PlayerUnit.playerUnit.jumpSound);
        rb.velocity = new Vector2(rb.velocity.x, 0);
        rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        GetComponent<Animator>().SetBool("IsJumping", true);
        jumpTimer = 0;
    }

    void ModifyPhysics()
    {
        bool directionChange = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);
        if (isOnGround)
        {
            if (Mathf.Abs(direction.x) < 0.4f || directionChange)
                rb.drag = linearDrag;
            else
                rb.drag = 0;
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.5f;

            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallPower;
            }
            else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
            {
                rb.gravityScale = gravity * (fallPower * 0.2f);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            if (extraLife)
            {
                extraLife = false;
                rayLenght = 0.7f;
                GetComponent<Animator>().SetBool("IsBig", false);
            }
            else
            {
                PlayerUnit.playerUnit.bsoAudio.Stop();
                PlayerUnit.playerUnit.playerFX.PlayOneShot(PlayerUnit.playerUnit.marioDies);
                PlayerUnit.playerUnit.currentLifes -= 1;
                StartCoroutine(Died());
            }
        }

        if(collision.gameObject.tag == "Mushroom")
        {
            rayLenght = 1f;
            PlayerUnit.playerUnit.playerFX.PlayOneShot(PlayerUnit.playerUnit.powerUp);
            GetComponent<Animator>().SetBool("IsBig", true);
            Destroy(collision.gameObject);
            ScoreSystem.score.playerScore += mushroomScore;
            DataManager.dataManager.actualScore = ScoreSystem.score.playerScore;
            extraLife = true;
        }
    }

    private void Raycasting()
    {
        RaycastHit2D hitUpRayRight = Physics2D.Raycast(transform.position + colliderOffset, Vector2.up, rayLenght, boxLayer);
        RaycastHit2D hitUpRayLeft = Physics2D.Raycast(transform.position - colliderOffset, Vector2.up, rayLenght, boxLayer);

        isOnCeiling = hitUpRayLeft || hitUpRayRight;

        if(hitUpRayLeft.collider != null || hitUpRayRight.collider != null)
        {
            RaycastHit2D hitRay = hitUpRayRight;

            if (hitUpRayLeft)
                hitRay = hitUpRayLeft;
            else if (hitUpRayRight)
                hitRay = hitUpRayRight;

            if (hitRay.collider.tag == "Brick" && extraLife)
            {
                Instantiate(particles, hitRay.collider.transform);
                PlayerUnit.playerUnit.playerFX.PlayOneShot(PlayerUnit.playerUnit.breakSound);
                hitRay.collider.GetComponent<SpriteRenderer>().enabled = false;
                hitRay.collider.GetComponent<BoxCollider2D>().enabled = false;
            }
            else if(hitRay.collider.tag == "Brick" && !extraLife)
            {
                PlayerUnit.playerUnit.playerFX.PlayOneShot(PlayerUnit.playerUnit.bounceSound);
                hitRay.collider.GetComponent<Brick>().BlockBounce();
            }

            if (hitRay.collider.tag == "Lootbox")
            {
                var lbComponents = hitRay.collider.GetComponent<LootBrick>();
                PlayerUnit.playerUnit.playerFX.PlayOneShot(PlayerUnit.playerUnit.bounceSound);
                lbComponents.BlockBounce();
                lbComponents.LootBlock.SetActive(false);
                lbComponents.EmptyBlock.SetActive(true);
                if (lbComponents.HaveMushroom)
                {
                    lbComponents.HaveMushroom = false;
                    PlayerUnit.playerUnit.playerFX.PlayOneShot(PlayerUnit.playerUnit.mushroomShowup);
                    lbComponents.Mushroom.SetActive(true);
                }
                else if (lbComponents.HasCoin)
                {
                    lbComponents.HasCoin = false;
                    PlayerUnit.playerUnit.playerFX.PlayOneShot(PlayerUnit.playerUnit.coinSound);
                    lbComponents.Coin.SetActive(true);
                    ScoreSystem.score.playerScore += coinScore;
                    DataManager.dataManager.actualScore = ScoreSystem.score.playerScore;
                }
            }
        }

        if (isOnEnemy)
        {
            GetComponent<Rigidbody2D>().AddForce(new Vector2(1, enemyKill));
        }
    }
    

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.down * rayLenght);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.down * rayLenght);
        Gizmos.DrawLine(transform.position + colliderOffset, transform.position + colliderOffset + Vector3.up * rayLenght);
        Gizmos.DrawLine(transform.position - colliderOffset, transform.position - colliderOffset + Vector3.up * rayLenght);
    }
}
