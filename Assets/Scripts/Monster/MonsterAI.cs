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


    [HideInInspector]
    public int floorId { get; private set; }
    [HideInInspector]
    public Transform currentFloor { get; private set; }
    [HideInInspector]
    public Transform blocks { get; private set; }
    [HideInInspector]
    public Transform ladders { get; private set; }

    public Transform floors;
    public GameObject prefabDestroyParticle;

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
        if (id < floors.childCount && id >= 0)
        {
            floorId = id;
            currentFloor = floors.GetChild(id);
            blocks = currentFloor.GetChild(0);
            ladders = currentFloor.GetChild(1);
        }
    }

    void Update()
    {
        sm.CurState.Update();
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
