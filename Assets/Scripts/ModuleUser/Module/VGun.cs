using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VGun : Canon
{
    //protected Transform _transform;
    [SerializeField] Transform[] _canonSpawnArray;

    void Start()
    {
        Transform _firstChild = transform.GetChild(0).transform;
        _canonSpawnArray = _firstChild.GetComponentsInChildren<Transform>();
    }

    protected override void Shoot()
    {
        //TO-DO: Problem with the array, take the transform of the first child
        for (int i = 1; i < _canonSpawnArray.Length; i++)
        {
            Shot lShot = Instantiate(_prefabShot, _canonSpawnArray[i].position, Quaternion.identity).GetComponent<Shot>();
            lShot.SetUp(isEnemy, _canonSpawnArray[i].rotation, GetSpeed(), _hitValue);
        }
        
        _canShoot = false;
        Invoke("CanShootAgain", _shotRate);
    }
}
