using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class MonsterAI : MonoBehaviour
{
    private StateMachine sm;

    [HideInInspector]
    public Rigidbody2D rb2D;
    [HideInInspector]
    public BoxCollider2D boxCol2D;
    [HideInInspector]
    public Animator anim;
    [HideInInspector]
    public SpriteRenderer spr;

    public float speed = 250f;

    public Slider sliderEnergy;
    private float timeEnergy = 0;

    public Block currentBlock;

    KeyCode[] keyCodesSequence = {KeyCode.UpArrow, KeyCode.DownArrow,
                                  KeyCode.LeftArrow, KeyCode.RightArrow,
                                  KeyCode.Space};


    void Awake()
    {
        sm = new StateMachine();
    }

    void Start()
    {
        ResetAI();
    }

    public void ResetAI(){
        sm.CurState = new Idle(gameObject, sm);

        rb2D = GetComponent<Rigidbody2D>();
        boxCol2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();

        sliderEnergy.value = 0f;
        timeEnergy = 0;

        // Ladder
        if (rb2D.gravityScale == 0)
        {
            Transform ladderLeft = GameManager.instance.playerFloor.GetComponent<FloorManager>().ladders.GetChild(0);
            Transform ladderRight = GameManager.instance.playerFloor.GetComponent<FloorManager>().ladders.GetChild(1);

            float dLeft = Vector2.Distance(ladderLeft.position, transform.position);
            float dRight = Vector2.Distance(ladderRight.position, transform.position);

            Transform ladder;
            if (dLeft < dRight)
            {
                ladder = ladderLeft;
            }
            else
            {
                ladder = ladderRight;
            }

            sm.CurState = new StateLadder(gameObject, sm, ladder);

        } else {
            // Já achar o bloco mais perto
        }
    }

    void Update()
    {
        InputToReturnNormal();
        sm.CurState.Update();
    }

    private void InputToReturnNormal()
    {
        timeEnergy += Time.deltaTime;
        if (timeEnergy > 0.01f)
        {
            timeEnergy = 0f;
            sliderEnergy.value -= 0.01f;
        }

        for (int i = 0; i < keyCodesSequence.Length; i++)
        {
            if (Input.GetKeyDown(keyCodesSequence[i]))
                sliderEnergy.value += 0.05f;
        }

        if (sliderEnergy.value == sliderEnergy.maxValue)
        {
            ReturnNormal();
        }
    }

    private void ReturnNormal()
    {
        this.enabled = false;
        rb2D.velocity = new Vector2(0f, rb2D.velocity.y);
        GameManager.instance.BackNormal();
    }

    void FixedUpdate()
    {
        sm.CurState.FixedUpdate();
    }

    private void OnTriggerStay2D(Collider2D other)
    {

        if (!GetComponent<MonsterAI>().enabled)
        {
            return;
        }

        if (other.gameObject.tag == "Ladder")
        {
            // In Ladder
            if (rb2D)
                rb2D.gravityScale = 0;

            // Current Floor
            GameManager.instance.ChangeFloor(other.transform.parent.parent);

        }
        else if (other.gameObject.tag == "Wall")
        {
            // Current Floor
            GameManager.instance.ChangeFloor(other.transform.parent.parent);

            if (!currentBlock && sm.CurState.GetType() == typeof(Idle))
            {
                if (other.GetComponent<SpriteRenderer>().enabled)
                    currentBlock = other.GetComponent<Block>();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {

        if (other.gameObject.tag == "Ladder")
        {
            if (rb2D)
                rb2D.gravityScale = 1;
        }

    }
}
