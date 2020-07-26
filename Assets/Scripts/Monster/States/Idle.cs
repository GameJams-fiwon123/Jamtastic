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

        monsterAI.rb2D.velocity = dir;
    }

    public override void Update()
    {
        time += Time.deltaTime;
        MonsterAI monsterAI = go.GetComponent<MonsterAI>();

        monsterAI.anim.Play("Idle");

        if (time > 0.25f)
        {
            time = 0f;

            if (monsterAI.currentBlock)
                this.sm.CurState = new Walk(this.go, this.sm, null);
            else
            {
                if (GameManager.instance.playerFloor.GetComponent<FloorManager>().id == 0 && !monsterAI.currentBlock)
                {
                    monsterAI.ReturnNormal();
                }

                Transform ladderLeft = GameManager.instance.playerFloor.GetComponent<FloorManager>().ladders.GetChild(0);
                Transform ladderRight = GameManager.instance.playerFloor.GetComponent<FloorManager>().ladders.GetChild(1);

                float dLeft = Vector2.Distance(ladderLeft.position, monsterAI.transform.position);
                float dRight = Vector2.Distance(ladderRight.position, monsterAI.transform.position);

                Transform ladder;
                if (dLeft < dRight)
                {
                    ladder = ladderLeft;
                }
                else
                {
                    ladder = ladderRight;
                }

                this.sm.CurState = new Walk(this.go, this.sm, ladder);
            }
        }
    }

}
