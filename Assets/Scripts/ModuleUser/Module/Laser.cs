﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Module
{
    [Header("Gameplay Datas")]
    [SerializeField] protected float _speedShot = 2f;
    [SerializeField] float _fireRate = 3f;

    [SerializeField] protected LaserShot _laserShot;


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
        if (_canShoot) Fire();
    }

    protected virtual void Fire()
    {
        _laserShot.ActiveMode();
        _canShoot = false;
        Invoke("Disable", _fireRate);
    }

    protected virtual void Disable()
    {
        _laserShot.DesactiveMode();
        Invoke("CanShootAgain", _fireRate);
    }

    protected virtual void CanShootAgain()
    {
        _canShoot = true;
    }
}
