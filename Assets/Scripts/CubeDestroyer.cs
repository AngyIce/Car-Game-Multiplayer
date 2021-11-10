using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroyer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Explosion.haveCollided == true)
        StartCoroutine(ExecuteAfterTime(20));
    }

    IEnumerator ExecuteAfterTime(float time)
    {
        Explosion.haveCollided = false;
        yield return new WaitForSeconds(time);

        GameObject[] pieces = GameObject.FindGameObjectsWithTag("Blocks");
        foreach (GameObject Blocks in pieces)
            GameObject.Destroy(Blocks);
        
    }
}
