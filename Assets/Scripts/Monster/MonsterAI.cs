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

    [HideInInspector]
    public int floorId { get; private set; }
    [HideInInspector]
    public Transform currentFloor { get; private set; }
    [HideInInspector]
    public Transform blocks { get; private set; }
    [HideInInspector]
    public Transform ladders { get; private set; }

    public GameObject prefabDestroyParticle;
    public Slider sliderEnergy;
    private float timeEnergy = 0;

    public Block currentBlock;

    KeyCode[] keyCodesSequence = {KeyCode.UpArrow, KeyCode.DownArrow, 
                                  KeyCode.LeftArrow, KeyCode.RightArrow,
                                  KeyCode.Space};


    void Awake()
    {
        sm = new StateMachine();
        sm.CurState = new Idle(gameObject, sm);
    }

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCol2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        floorId = GameManager.instance.playerFloor.GetComponent<FloorManager>().id;
        ChangeFloor(floorId);
        sliderEnergy.value = 0f;
    }

    public void ChangeFloor(int id)
    {
        if (id < GameManager.instance.floors.childCount && id >= 0)
        {
            floorId = id;
            currentFloor = GameManager.instance.floors.GetChild(id);
            blocks = currentFloor.GetChild(0);
            ladders = currentFloor.GetChild(1);
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

        for (int i = 0; i < keyCodesSequence.Length; i++){
            if (Input.GetKeyDown(keyCodesSequence[i]))
                sliderEnergy.value += 0.05f;
        }

        if (sliderEnergy.value == sliderEnergy.maxValue){
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

    public void DestroyBlock()
    {
        currentBlock.GetComponent<SpriteRenderer>().enabled = false;
        GameObject particleObject = Instantiate(prefabDestroyParticle, transform.position, Quaternion.identity);
        Destroy(particleObject, 1f);
        SearchBlocksInFloor();
    }

    private void SearchBlocksInFloor()
    {
        int findLeft = 0;
        Block leftBlockAux = currentBlock.leftBlock;
        while(leftBlockAux && !leftBlockAux.GetComponent<SpriteRenderer>().enabled){
            leftBlockAux = leftBlockAux.leftBlock;
            findLeft++;
        }

        int findRight = 0;
        Block rightBlockAux = currentBlock.rightBlock;
        while(rightBlockAux && !rightBlockAux.GetComponent<SpriteRenderer>().enabled){
            rightBlockAux = rightBlockAux.rightBlock;
            findRight++;
        }

        if ((findLeft > findRight && rightBlockAux) || (rightBlockAux && !leftBlockAux)){
            currentBlock = rightBlockAux;
        } else if ((findLeft < findRight && leftBlockAux) || (!rightBlockAux && leftBlockAux)){
            currentBlock = leftBlockAux;
        } else if (leftBlockAux && rightBlockAux){
            currentBlock = (Random.Range(0, 2) == 0) ? leftBlockAux : rightBlockAux;
        } else {
            currentBlock = null;
        }

    }

    private void OnTriggerStay2D(Collider2D other) {

        if(other.gameObject.tag == "Ladder") {
            GameManager.instance.ChangeFloor(other.transform.parent.parent);
            ChangeFloor(GameManager.instance.playerFloor.GetComponent<FloorManager>().id);
            if (!currentBlock && sm.CurState.GetType() == typeof(Idle))
                currentBlock = other.GetComponent<Ladder>().block;

        } else if (other.gameObject.tag == "Wall"){
            if (!currentBlock && sm.CurState.GetType() == typeof(Idle))
                currentBlock = other.GetComponent<Block>();
        }
    }
}
