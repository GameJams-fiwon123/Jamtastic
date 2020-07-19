using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
    private float time = 0f;

    public Idle(GameObject go, StateMachine sm) : base(go, sm)
    {
        this.go = go;
        this.sm = sm;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        MonsterAI monsterAI = go.GetComponent<MonsterAI>();

        Vector2 dir = Vector2.zero;
        dir.y = monsterAI.rb2D.velocity.y;

        monsterAI.rb2D.velocity =  dir;
    }

    public override void Update()
    {
        time += Time.deltaTime;
        MonsterAI monsterAI = go.GetComponent<MonsterAI>();
        if (time > 0.25f){
            time = 0f;

            if (monsterAI.blocks.childCount > 0)
                this.sm.CurState = new Walk(this.go, this.sm, null);
            else{
                int indexLadder = Random.Range(0, monsterAI.ladders.childCount);
                Transform ladder = monsterAI.ladders.GetChild(indexLadder);
                this.sm.CurState = new Walk(this.go, this.sm, ladder);
            }
        }
    }

}
