using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateLadder : State
{
    private Transform ladder;

    public StateLadder(GameObject go, StateMachine sm, Transform ladder) : base(go, sm)
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

        Vector3 nextPos = ladder.position;
        nextPos.y -= 0.25f;

        Vector2 dir = Vector2.zero;
        dir = nextPos - monsterAI.transform.position;

        monsterAI.rb2D.velocity = dir.normalized * monsterAI.speed * Time.deltaTime;
    }

    public override void Update()
    {
        MonsterAI monsterAI = go.GetComponent<MonsterAI>();

        Vector3 nextPos = ladder.position;
        nextPos.y -= 0.25f;

        float d = Vector2.Distance(go.transform.position, nextPos);

        if (d < 0.5f)
        {
            monsterAI.currentBlock = ladder.GetComponent<Ladder>().block;
            if (!monsterAI.currentBlock.GetComponent<SpriteRenderer>().enabled)
                SearchBlocksInFloor();

            this.sm.CurState = new Walk(this.go, this.sm, null);

        }
    }

    private void SearchBlocksInFloor()
    {
        MonsterAI monsterAI = go.GetComponent<MonsterAI>();
        int findLeft = 0;
        Block leftBlockAux = monsterAI.currentBlock.leftBlock;
        while (leftBlockAux && !leftBlockAux.GetComponent<SpriteRenderer>().enabled)
        {
            leftBlockAux = leftBlockAux.leftBlock;
            findLeft++;
        }

        int findRight = 0;
        Block rightBlockAux = monsterAI.currentBlock.rightBlock;
        while (rightBlockAux && !rightBlockAux.GetComponent<SpriteRenderer>().enabled)
        {
            rightBlockAux = rightBlockAux.rightBlock;
            findRight++;
        }

        if ((findLeft > findRight && rightBlockAux) || (rightBlockAux && !leftBlockAux))
        {
            monsterAI.currentBlock = rightBlockAux;
        }
        else if ((findLeft < findRight && leftBlockAux) || (!rightBlockAux && leftBlockAux))
        {
            monsterAI.currentBlock = leftBlockAux;
        }
        else if (leftBlockAux && rightBlockAux)
        {
            monsterAI.currentBlock = (Random.Range(0, 2) == 0) ? leftBlockAux : rightBlockAux;
        }
        else
        {
            monsterAI.currentBlock = null;
        }

    }
}
