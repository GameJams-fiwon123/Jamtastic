using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Block leftBlock;
    public Block rightBlock;
    public GameObject prefabDestroyParticle;

    public void DestroyBlock(){
        GetComponent<SpriteRenderer>().enabled = false;
        GameObject objParticle = Instantiate(prefabDestroyParticle, transform.position, transform.rotation);
        Destroy(objParticle, 1f);
    }
}