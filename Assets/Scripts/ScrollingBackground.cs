using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public Vector3 scrollingVector = new Vector3(0,-1,0);
    private Transform _transform;

    public void Start()
    {
        _transform = transform;
    }
    // Update is called once per frame
    void Update()
    {
         Vector3 lMovement = scrollingVector * Time.deltaTime;
        _transform.Translate(lMovement);
    }
}
