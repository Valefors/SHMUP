using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTraveling : MonoBehaviour
{

    public float speed;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
        Vector3 newCoord = new Vector3(transform.position.x, transform.position.y, transform.position.z+speed);
        transform.SetPositionAndRotation(newCoord, transform.rotation);
        //transform.Translate(Vector3.right * speed);

        //rb.MovePosition(newCoord);
    }
}
