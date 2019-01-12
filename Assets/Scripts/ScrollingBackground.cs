using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    public Vector3 scrollingVector;
    private Transform _transform;

    public void Start()
    {
        _transform = transform;
        scrollingVector = GameManager.manager.scrollingVector;
    }
    // Update is called once per frame
    void Update()
    {
        if (GameManager.manager.isPause) return;
        Vector3 lMovement = scrollingVector * Time.deltaTime;
        _transform.Translate(lMovement);
    }
}
