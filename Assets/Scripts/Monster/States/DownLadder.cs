using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownLadder : State
{
    Transform ladder;
    float time = 0f;

    public DownLadder(GameObject go, StateMachine sm, Transform ladder) : base(go, sm)
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
        dir.y = -1;

        monsterAI.rb2D.velocity = dir * monsterAI.speed * Time.deltaTime;
    }

    public override void Update()
    {
        time += Time.deltaTime;
        MonsterAI monsterAI = go.GetComponent<MonsterAI>();

        Vector3 nextPos = ladder.position;
        nextPos.y -= 1f;
        float d = Vector2.Distance(go.transform.position, nextPos);
        if (d < 0.5f)
        {
            monsterAI.ChangeFloor(monsterAI.floorId-1);
            monsterAI.rb2D.velocity = Vector2.zero;
            this.sm.CurState = new Walk(this.go, this.sm, null);
        } else if (time > 2f){
            time = 0f;
            this.sm.CurState = new Walk(this.go, this.sm, null);
        }
    }
}
