using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : ShooterModule
{
    [Header("Shot data")]
    [SerializeField] protected float _shotRate = 0.1f;
    [SerializeField] protected float _speedShot = 2f;
    [SerializeField] protected float _speedShotFactor = 2f;
    [SerializeField] protected int _hitValue = 1;

    [SerializeField] protected GameObject _prefabShot;



    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    public override void DoActionNormal()
    {
        if (_canShoot) Shoot();
    }

    protected virtual void Shoot()
    {
        Shot lShot = Instantiate(_prefabShot, _transform.position, Quaternion.identity).GetComponent<Shot>();
        lShot.SetUp(isEnemy, _transform.rotation, GetSpeed(), _hitValue);
        _canShoot = false;
        Invoke("CanShootAgain", _shotRate);
    }

    protected float GetSpeed()
    {
        return (isEnemy ? _speedShot : _speedShot * _speedShotFactor);
    }
    
    protected virtual void CanShootAgain()
    {
        _canShoot = true;
    }
}

