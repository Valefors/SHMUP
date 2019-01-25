using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTraveling : MonoBehaviour
{

    public float speed;
    Rigidbody rb;
    public bool isCollapseTrigger=false;
    float spentTime = 0;
    public float acceleration;
    public float timeToWait;
    public float timeToPush;
    public bool isActor=false;
    ParticleSystem[] temp;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if(isActor)
        {
            temp= GetComponentsInChildren<ParticleSystem>();
            Debug.Log(temp.Length);
            for (int i = 0; i < temp.Length; i++)
            {
                temp[i].Simulate(1);
                temp[i].Play();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isActor)
        {
          /*  for (int i = 0; i < temp.Length; i++)
            {
                temp[i].Simulate(1);
              //  temp[i].Play();
            }*/
        }
        if (!isCollapseTrigger)
        {
            Vector3 newCoord = new Vector3(transform.position.x, transform.position.y, transform.position.z + (speed*Time.deltaTime));
            transform.SetPositionAndRotation(newCoord, transform.rotation);
        }
        else
        {
            spentTime += Time.deltaTime;
            if(spentTime>=timeToWait && spentTime<=timeToWait+timeToPush)
            {
                Vector3 newCoord = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);
                transform.SetPositionAndRotation(newCoord, transform.rotation);
                speed += acceleration;
            }
        }
    }
}
