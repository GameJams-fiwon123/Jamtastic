using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class MonsterAI : MonoBehaviour
{
    StateMachine sm;

    [HideInInspector]
    public Rigidbody2D rb2D;
    [HideInInspector]
    public BoxCollider2D boxCol2D;

    public float speed = 250f;

    public Transform blocks;

    void Awake()
    {
        sm = new StateMachine();
        sm.CurState = new Idle(gameObject, sm);
    }

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        boxCol2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        sm.CurState.Update();
    }

    void FixedUpdate()
    {
        sm.CurState.FixedUpdate();
    }

    public void DestroyBlock(){
        Destroy(blocks.GetChild(0).gameObject);
    }
}
