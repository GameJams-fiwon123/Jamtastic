using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Idle : State
{
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
        MonsterAI monsterAI = go.GetComponent<MonsterAI>();
        if (monsterAI.blocks != null){
            this.sm.CurState = new Walk(this.go, this.sm);
        }
    }

}
