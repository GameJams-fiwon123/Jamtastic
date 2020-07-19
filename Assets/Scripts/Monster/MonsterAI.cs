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
        ChangeFloor(floorId);
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
        sliderEnergy.gameObject.SetActive(false);
    }

    void FixedUpdate()
    {
        sm.CurState.FixedUpdate();
    }

    public void DestroyBlock()
    {
        Destroy(blocks.GetChild(0).gameObject);
        GameObject particleObject = Instantiate(prefabDestroyParticle, transform.position, Quaternion.identity);
        Destroy(particleObject, 1f);
    }
}
