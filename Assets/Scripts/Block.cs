using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Block leftBlock;
    public Block rightBlock;
    public GameObject prefabDestroyParticle;

    public AudioSource audio;
    public Animator anim;

    private float progress = 0f;

    public void DestroyBlock()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GameObject objParticle = Instantiate(prefabDestroyParticle, transform.position, transform.rotation);
        GameManager.instance.DestroyWall();
        Destroy(objParticle, 1f);
        progress = 0f;
        anim.Play("LoseScore");
        audio.Play();
    }

    public void Build(float playerBuild)
    {
        if (progress < 100f)
        {
            progress += playerBuild;
        }
        else if (progress >= 100f)
        {
             anim.Play("GainScore");
            GameManager.instance.BuildWall();
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}