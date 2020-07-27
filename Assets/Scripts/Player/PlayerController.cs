using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float leftBorder;
    public float rightBorder;

    private float moveInputX;
    private float moveInputY;

    private bool isOnLadder;
    private bool isOnWall;
    private bool isBuilding;
    private float timeToBuild;

    private Rigidbody2D rb;
    private BoxCollider2D bc2d;
    private GameObject wall;
    private Animator anim;
    private SpriteRenderer spr;

    public AudioSource audioBuilding;
    public AudioClip[] clipsBuilding;


    void Start()
    {

        spr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();

    }

    void Update()
    {

        if (GameManager.instance.isStarted)
        {
            moveInputX = Input.GetAxis("Horizontal");
            moveInputY = Input.GetAxis("Vertical");

            isBuilding = Input.GetButton("Build");
        }

    }

    private void FixedUpdate()
    {
        if (!GameManager.instance.isStarted)
        {
            return;
        }

        if (!isBuilding)
        {

            if ((rb.position.x <= leftBorder && moveInputX < 0) || (rb.position.x >= rightBorder && moveInputX > 0))
            {

                rb.velocity = new Vector2(moveInputX * 0, rb.velocity.y);

            }
            else
            {

                if (moveInputX > 0)
                {
                    spr.flipX = false;
                    anim.Play("Walk");
                }
                else if (moveInputX < 0)
                {
                    spr.flipX = true;
                    anim.Play("Walk");
                }
                else
                {
                    anim.Play("Idle");
                }
                rb.velocity = new Vector2(moveInputX * speed, rb.velocity.y);

            }

        }
        else if (isBuilding)
        {

            rb.velocity = new Vector2(0, 0);

            if (isOnWall && !wall.GetComponent<SpriteRenderer>().enabled)
            {
                anim.Play("Working");
                wall.GetComponent<Block>().Build(66f * Time.deltaTime);

                if (!audioBuilding.isPlaying)
                {
                    audioBuilding.clip = clipsBuilding[Random.Range(0, clipsBuilding.Length)];
                    audioBuilding.Play();
                }
            }
            else
            {
                audioBuilding.Stop();
                anim.Play("Idle");
            }

        }

        if (isOnLadder)
        {

            rb.gravityScale = 0;

            if (moveInputY == 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
            else if (moveInputY != 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, moveInputY * speed);
            }

        }
        else if (isOnLadder == false)
        {

            rb.gravityScale = 1;

        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (other.gameObject.tag == "Ladder")
        {
            GameManager.instance.ChangeFloor(other.transform.parent.parent);
            isOnLadder = true;
        }

        if (other.gameObject.tag == "Wall")
        {
            GameManager.instance.ChangeFloor(other.transform.parent.parent);
            isOnWall = true;
            wall = other.gameObject;
        }

    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.tag == "Ladder")
        {
            isOnLadder = false;
        }

        if (other.gameObject.tag == "Wall")
        {
            isOnWall = false;
            wall = null;
            timeToBuild = 0;
        }

    }

}
