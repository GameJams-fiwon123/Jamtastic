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
        DestroyBlock();
        SearchBlocksInFloor();
    }

    public void DestroyBlock()
    {
        MonsterAI monsterAI = go.GetComponent<MonsterAI>();
        monsterAI.currentBlock.GetComponent<Block>().DestroyBlock();
    }

    private void SearchBlocksInFloor()
    {
        MonsterAI monsterAI = go.GetComponent<MonsterAI>();
        int findLeft = 0;
        Block leftBlockAux = monsterAI.currentBlock.leftBlock;
        while(leftBlockAux && !leftBlockAux.GetComponent<SpriteRenderer>().enabled){
            leftBlockAux = leftBlockAux.leftBlock;
            findLeft++;
        }

        int findRight = 0;
        Block rightBlockAux = monsterAI.currentBlock.rightBlock;
        while(rightBlockAux && !rightBlockAux.GetComponent<SpriteRenderer>().enabled){
            rightBlockAux = rightBlockAux.rightBlock;
            findRight++;
        }

        if ((findLeft > findRight && rightBlockAux) || (rightBlockAux && !leftBlockAux)){
            monsterAI.currentBlock = rightBlockAux;
        } else if ((findLeft < findRight && leftBlockAux) || (!rightBlockAux && leftBlockAux)){
            monsterAI.currentBlock = leftBlockAux;
        } else if (leftBlockAux && rightBlockAux){
            monsterAI.currentBlock = (Random.Range(0, 2) == 0) ? leftBlockAux : rightBlockAux;
        } else {
            monsterAI.currentBlock = null;
            if (GameManager.instance.playerFloor.GetComponent<FloorManager>().id == 0){
                monsterAI.ReturnNormal();
            }
        }

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
