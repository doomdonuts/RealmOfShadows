using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowardsEnemyWhenAlert : MonoBehaviour
{
    private Transform player;
    public float speed;
    public bool alerted;
    public float alertTime;
    public float resetAlertTime;

    private GameManager gameManager;


    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private EnemyBehavior enemyBehavior;
    private AudioSource audioSource;
    private EnemyBehavior.EnemyMovementDelegate movementMethod;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        gameManager = GameObject.FindObjectOfType<GameManager>();
        enemyBehavior = gameObject.GetComponent<EnemyBehavior>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        animator = this.GetComponent<Animator>();

        audioSource = this.GetComponent<AudioSource>();
        movementMethod = standardEnemyMovement;
        resetAlertTime = alertTime;

    }

    void Update()
    {
        enemyBehavior.ExecuteEnemyMovement(movementMethod);
    }

    // Start is called before the first frame update
    public void standardEnemyMovement()
    {
        if (!GameManager.isPaused && alerted)
        {
            float xMovement;
            float yMovement;

            if (this.transform.position.x > player.position.x)
            {
                xMovement = -1 * speed * Time.deltaTime;
                spriteRenderer.flipX = true;
            }
            else
            {
                xMovement = speed * Time.deltaTime;
                spriteRenderer.flipX = false;
            }

            if (this.transform.position.y > player.position.y)
            {
                yMovement = -1 * speed * Time.deltaTime;
            }
            else
            {
                yMovement = speed * Time.deltaTime;
            }

            this.transform.position = new Vector2(this.transform.position.x + xMovement, this.transform.position.y + yMovement);
            animator.enabled = true;
            if (!audioSource.isPlaying) audioSource.Play();

            alertTime -= Time.deltaTime;
            if (alerted && alertTime <= 0) alerted = false;
        }
        else
        {
            if (audioSource.isPlaying) audioSource.Stop();
            animator.enabled = false;
            alertTime = resetAlertTime;
        }
    }
}
