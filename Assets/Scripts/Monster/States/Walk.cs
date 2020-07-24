﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walk : State
{
    private Transform ladder;

    Vector2 dir = Vector2.zero;

    public Walk(GameObject go, StateMachine sm, Transform ladder) : base(go, sm)
    {
        this.go = go;
        this.sm = sm;
        this.ladder = ladder;
    }

    public override void Enter()
    {
        // Nothing
    }

    public override void Exit()
    {
        // Nothing
    }

    public override void FixedUpdate()
    {
        MonsterAI monsterAI = go.GetComponent<MonsterAI>();

        // If exists blocks to destroy
        if (monsterAI.currentBlock && monsterAI.currentBlock.GetComponent<SpriteRenderer>().enabled)
        {

            // Distance of block
            Transform block = monsterAI.currentBlock.transform;
            float d = Vector2.Distance(go.transform.position, block.position);

            if (d > 0.5f)
            {
                dir = Vector2.zero;

                // dir.x
                dir.x = block.position.x - go.transform.position.x;
                dir.x = (dir.x >= 0) ? 1f : -1f;
                dir.x = dir.x * monsterAI.speed * Time.deltaTime;

                // dir.y
                dir.y = monsterAI.rb2D.velocity.y;

                // Velocity
                monsterAI.rb2D.velocity = dir;
            }
        }
        else if (ladder)
        {
            // Distance of ladder
            float d = Vector2.Distance(go.transform.position, ladder.position);

            if (d > 0.5f)
            {
                dir = Vector2.zero;

                // dir.x
                dir.x = ladder.position.x - go.transform.position.x;
                dir.x = (dir.x >= 0) ? 1f : -1f;
                dir.x = dir.x * monsterAI.speed * Time.deltaTime;

                // dir.y
                dir.y = monsterAI.rb2D.velocity.y;

                // Velocity
                monsterAI.rb2D.velocity = dir;
            }
        }
    }

    public override void Update()
    {
        MonsterAI monsterAI = go.GetComponent<MonsterAI>();

        monsterAI.anim.Play("Walk");

        if (dir.x > 0){
            monsterAI.spr.flipX = false;
        } else {
            monsterAI.spr.flipX = true;
        }

        if (monsterAI.currentBlock && monsterAI.currentBlock.GetComponent<SpriteRenderer>().enabled)
        { // Exists block
            Transform block = monsterAI.currentBlock.transform;
            float d = Vector2.Distance(go.transform.position, block.position);

            if (d <= 0.5f)
            {
                this.sm.CurState = new Attack(this.go, this.sm);
            }
        }
        else if (ladder)
        {

            float d = Vector2.Distance(go.transform.position, ladder.position);

            if (d < 0.5f)
            {
                // Always down
                int rangeNumber = Random.Range(0, 2);
                if (rangeNumber == 0)
                {
                    this.sm.CurState = new DownLadder(this.go, this.sm, ladder);
                }
                else
                {
                    this.sm.CurState = new UpLadder(this.go, this.sm, ladder);
                }

            }


        }
        else
        {
            this.sm.CurState = new Idle(this.go, this.sm);
        }

    }
}
