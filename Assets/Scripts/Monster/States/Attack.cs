using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : State
{
    private float time = 0f;

    public Attack(GameObject go, StateMachine sm) : base(go, sm)
    {
        this.go = go;
        this.sm = sm;
    }

    public override void Enter()
    {
        MonsterAI monsterAI = go.GetComponent<MonsterAI>();
        monsterAI.DestroyBlock();
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

        monsterAI.rb2D.velocity = dir;
    }

    public override void Update()
    {
        time += Time.deltaTime;

        if (time > 1f)
        {
            time = 0f;
            sm.CurState = new Walk(go, sm, null);
        }
    }

}
