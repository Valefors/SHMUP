using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] int ANGLE_ROTATION = 45;
    bool _isTaken = false;

    GameObject _module;
    
    [SerializeField] GameObject[] _modulesList;
    
    public void RotateLeft()
    {
        if(_isTaken) _module.transform.Rotate(0, 0, -ANGLE_ROTATION);
    }

    public void RotateRight()
    {
        if (_isTaken) _module.transform.Rotate(0, 0, ANGLE_ROTATION);
    }

    public void AddModule1()
    {
        if (_isTaken) return;

        _module = Instantiate(_modulesList[0], transform.position, transform.rotation, transform.parent.parent);
        _isTaken = true;
    }

    public void AddModule2()
    {
        if (_isTaken) return;

        _module = Instantiate(_modulesList[1], transform.position, transform.rotation, transform.parent.parent);
        _isTaken = true;
    }

    public void RemoveModule()
    {
        DestroyImmediate(_module);
        _isTaken = false;
    }
}
