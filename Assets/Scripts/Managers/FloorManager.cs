using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorManager : MonoBehaviour
{
    public int id = 0;
    public Transform blocks;
    public Transform ladders;
    public Transform beams;

    public Animator anim;

    bool isBuild = false;



    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (!isBuild){
            VerifyBlocks();
        }
    }

    private void VerifyBlocks()
    {
        foreach(Transform block in blocks){
            if (!block.gameObject.GetComponent<SpriteRenderer>().enabled){
                return;
            }
        }

        isBuild = true;
        if (id != 0)
            anim.Play("GainScore");
        GameManager.instance.SpawnFloor();

    }
}
