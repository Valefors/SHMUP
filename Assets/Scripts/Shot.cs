using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField]
    private float _speedShot;

    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        _transform.Translate(Vector3.right * _speedShot * Time.deltaTime);
    }
}
