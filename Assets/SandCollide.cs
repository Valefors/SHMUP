using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SandCollide : MonoBehaviour
{
    float timeSpent=0;
    public float timeToWait;

    // Update is called once per frame
    void Update()
    {
        timeToWait += Time.deltaTime;
        if(timeSpent>=timeToWait)
        {
            GetComponent<BoxCollider>().enabled = false;
        }
    }
}
