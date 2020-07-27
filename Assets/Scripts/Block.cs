using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Block leftBlock;
    public Block rightBlock;
    public GameObject prefabDestroyParticle;

    private float progress = 0f;

    public void DestroyBlock()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GameObject objParticle = Instantiate(prefabDestroyParticle, transform.position, transform.rotation);
        Destroy(objParticle, 1f);
        progress = 0f;
    }

    public void Build(float playerBuild)
    {
        if (progress < 100f)
        {
            progress += playerBuild;
        }
        else if (progress >= 100f)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}