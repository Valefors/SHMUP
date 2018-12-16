using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField]
    private float _speedShot = 1f;

    [SerializeField]
    private Vector3 _direction = Vector3.up;


    private Transform _transform;

    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        _transform.Translate(_direction * _speedShot * Time.deltaTime);
    }
}
