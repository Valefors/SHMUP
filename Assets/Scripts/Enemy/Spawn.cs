using System.Collections;
using System.Collections.Generic;
using UnityEditor;
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

       // _module = Instantiate(_modulesList[0], transform.position, transform.rotation, transform.parent.parent);
       // TEST PREFAB

        _module = PrefabUtility.InstantiatePrefab(_modulesList[0] as GameObject) as GameObject;

        _module.transform.parent = transform;

        _module.transform.position = transform.position;
        _module.transform.rotation = transform.rotation;

        _module.transform.localScale = _modulesList[0].transform.localScale;
        _isTaken = true;
    }

    public void AddModule2()
    {
        if (_isTaken) return;

        //_module = Instantiate(_modulesList[1], transform.position, transform.rotation, transform.parent.parent);

        _module = PrefabUtility.InstantiatePrefab(_modulesList[1] as GameObject) as GameObject;
        _module.transform.position = transform.position;
        _module.transform.rotation = transform.rotation;
        _module.transform.parent = transform;
        _module.transform.localScale = _modulesList[1].transform.localScale;
        _isTaken = true;
    }

    public void AddModule3()
    {
        if (_isTaken) return;

        //_module = Instantiate(_modulesList[2], transform.position, transform.rotation, transform.parent.parent);

        _module = PrefabUtility.InstantiatePrefab(_modulesList[2] as GameObject) as GameObject;
        _module.transform.position = transform.position;
        _module.transform.rotation = transform.rotation;
        _module.transform.parent = transform;
        _module.transform.localScale = _modulesList[2].transform.localScale;
        _isTaken = true;
    }

    public void RemoveModule()
    {
        DestroyImmediate(_module);
        _isTaken = false;
    }
}
