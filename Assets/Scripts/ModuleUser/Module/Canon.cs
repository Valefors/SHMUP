﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canon : Module
{
    [Header("Shot data")]
    [SerializeField] protected float _shotRate = 0.1f;
    [SerializeField] protected float _speedShotEnnemy = 2f;
    [SerializeField] protected float _speedShotPlayer = 2f;
    [SerializeField] protected int _hitValue = 1;

    [SerializeField] protected GameObject _prefabShot;

    public bool isEnemy = true;

    [Header("Personal Datas")]
    private Transform _transform;
    protected bool _canShoot = true;


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
        float lSpeedForThisShot = isEnemy ? _speedShotEnnemy : _speedShotPlayer;
        lShot.SetUp(isEnemy, _transform.rotation, lSpeedForThisShot, _hitValue);
        _canShoot = false;
        Invoke("CanShootAgain", _shotRate);
    }
    
    protected virtual void CanShootAgain()
    {
        _canShoot = true;
    }
}

