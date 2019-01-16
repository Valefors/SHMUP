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
        //Obsolete 
        if(_canonSpawnArray == null)
            _canonSpawnArray = _firstChild.GetComponentsInChildren<Transform>();
    }

    protected override void Shoot()
    {
        //TO-DO: Problem with the array, take the transform of the first child
        if (useBurst && !isEnemy) useBurst = false;
        if (!useBurst || ((isWaiting && waitingTime > _timeWaitBurst) || isShooting))
        {
            for (int i = 0; i < _canonSpawnArray.Length; i++)
            {
                Shot lShot = Instantiate(_prefabShot, _canonSpawnArray[i].position, Quaternion.identity).GetComponent<Shot>();
                lShot.SetUp(isEnemy, _canonSpawnArray[i].rotation, GetSpeed(), _hitValue);
            }

            _canShoot = false;
            Invoke("CanShootAgain", _shotRate);

            if (useBurst)
            {
                countedShots++;
                if (isWaiting)
                {
                    waitingTime = 0;
                    isWaiting = false;
                    isShooting = true;
                }
                if (countedShots >= _numberShotBurst && isShooting)
                {
                    countedShots = 0;
                    isShooting = false;
                    isWaiting = true;
                }
            }
        }
        else waitingTime += Time.deltaTime;
    }
}
