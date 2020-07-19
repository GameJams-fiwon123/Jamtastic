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
    private float timeToBuild;
    
    private Rigidbody2D rb;
    private BoxCollider2D bc2d;
    private GameObject wall;

    void Start() {

        rb = GetComponent<Rigidbody2D>();
        bc2d = GetComponent<BoxCollider2D>();

    }

    void Update() {

        moveInputX = Input.GetAxis("Horizontal");
        moveInputY = Input.GetAxis("Vertical");

        if(!Input.GetButton("Build")) {
                        
            if((rb.position.x <= leftBorder && moveInputX < 0)||(rb.position.x >= rightBorder && moveInputX > 0)) {

                rb.velocity = new Vector2(moveInputX * 0, rb.velocity.y);

            } else {

                rb.velocity = new Vector2(moveInputX * speed, rb.velocity.y);

            }

        } else if (Input.GetButton("Build")) {

            rb.velocity = new Vector2(0,0);

            if(isOnWall) {
                if(timeToBuild < 3) {
                    timeToBuild += Time.deltaTime;
                } else if(timeToBuild >= 3) {
                    wall.GetComponent<SpriteRenderer>().enabled = true;
                }
            }

        }

        if(isOnLadder) {

            rb.gravityScale = 0;

            if(moveInputY == 0) {
                rb.velocity = new Vector2(rb.velocity.x, 0);
            } else if(moveInputY != 0) {
                rb.velocity = new Vector2(rb.velocity.x, moveInputY * speed);
            }

        } else if(isOnLadder == false) {

            rb.gravityScale = 1;

        }

    }

    private void OnTriggerStay2D(Collider2D other) {

        if(other.gameObject.tag == "Ladder") {
            isOnLadder = true;
        }

        if(other.gameObject.tag == "Wall") {
            isOnWall = true;
            wall = other.gameObject;
        }

    }

    private void OnTriggerExit2D(Collider2D other) {
        
        if(other.gameObject.tag == "Ladder") {
            isOnLadder = false;
        }
        
        if(other.gameObject.tag == "Wall") {
            isOnWall = false;
            wall = null;
            timeToBuild = 0;
        }

    }

}
