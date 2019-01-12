using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Module
{
    [Header("Gameplay Datas")]
    [SerializeField] float _shotRate = 3f;

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
        Invoke("Disable", _shotRate);
    }

    protected virtual void Disable()
    {
        _laserShot.DesactiveMode();
        Invoke("CanShootAgain", _shotRate);
    }

    protected virtual void CanShootAgain()
    {
        _canShoot = true;
    public override void SetModeFree()
    {
        _laserShot.DesactiveMode();
        base.SetModeFree();
    }

}
