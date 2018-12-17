using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : Module
{
    [Header("Gameplay Datas")]
    [SerializeField]
    protected float _shotRate = 0.1f;

    [SerializeField]
    protected float _speedShot = 2f;

    [SerializeField]
    protected GameObject _prefabShot;

    public bool isEnemy = true;

    [Header("Personal Datas")]
    private Transform _transform;
    private bool _canShoot = true;


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
        Shot shotShot = Instantiate(_prefabShot, _transform.position, Quaternion.identity).GetComponent<Shot>();
        shotShot.SetUp(isEnemy, this.transform.rotation, _speedShot);
        _canShoot = false;
        Invoke("CanShootAgain", _shotRate);
    }

    protected virtual void CanShootAgain()
    {
        _canShoot = true;
    }
}

