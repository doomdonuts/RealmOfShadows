using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour

{

    private float speed;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    public float maxSpeed;
    public float acceleration;
    public float initialSpeed;
    // Start is called before the first frame update
    void Start()
    {
        speed = initialSpeed;
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        animator = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!GameManager.isPaused) {

            if (speed < maxSpeed) speed += acceleration * Time.deltaTime;
            else if (speed > maxSpeed) speed = maxSpeed;

            if (Input.GetKey(KeyCode.W))
            {
                this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y + speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.A))
            {
                this.transform.position = new Vector2(this.transform.position.x - speed * Time.deltaTime, this.transform.position.y);
                spriteRenderer.flipX = true;
            }

            if (Input.GetKey(KeyCode.S))
            {
                this.transform.position = new Vector2(this.transform.position.x, this.transform.position.y - speed * Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.D))
            {
                this.transform.position = new Vector2(this.transform.position.x + speed * Time.deltaTime, this.transform.position.y);
                spriteRenderer.flipX = false;
            }

            if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
            {
                speed = initialSpeed;
                var clipInfo = animator.GetCurrentAnimatorClipInfo(0);
                AnimationClip clip = clipInfo[0].clip;
                animator.PlayInFixedTime(clip.name, 0, 0.0f);
                //audioSource.Stop();
            } else
            {
                animator.enabled = true;
            }

        } else
        {
            animator.enabled = false;
        }
    }
}
