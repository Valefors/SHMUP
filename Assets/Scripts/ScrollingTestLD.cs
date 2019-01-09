using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTestLD : MonoBehaviour
{
    public float scrollingSpeed;

    // Update is called once per frame
    void Update()
    {
        transform.position += Vector3.down * scrollingSpeed * Time.deltaTime;
    }
}
