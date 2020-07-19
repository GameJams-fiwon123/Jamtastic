using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpLadder : State
{
    Transform ladder;

    public UpLadder(GameObject go, StateMachine sm, Transform ladder) : base(go, sm)
    {
        this.go = go;
        this.sm = sm;
        this.ladder = ladder;
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
        dir.y = 1;

        monsterAI.rb2D.velocity = dir * monsterAI.speed * Time.deltaTime;
    }

    public override void Update()
    {
        MonsterAI monsterAI = go.GetComponent<MonsterAI>();

        Vector3 nextPos = ladder.position;
        nextPos.y += 1f;
        float d = Vector2.Distance(go.transform.position, nextPos);
        if (d < 0.5f)
        {
            monsterAI.ChangeFloor(monsterAI.floorId+1);
            
            this.sm.CurState = new Walk(this.go, this.sm, null);
        }
    }
}
