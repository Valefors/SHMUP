using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : ShooterModule
{
    [Header("Gameplay Datas")]
    [SerializeField] float _shotRate = 3f;

    [SerializeField] protected LaserShot _laserShot;
    
    

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
        _laserShot.ActiveMode(isEnemy);

        if (_shotRate == 2) AkSoundEngine.PostEvent("Laser_launched2", gameObject);
        if (_shotRate == 4) AkSoundEngine.PostEvent("Laser_launched4", gameObject);

        _canShoot = false;
        Invoke("Disable", _shotRate);
    }

    protected virtual void Disable()
    {
        _laserShot.DesactiveMode();
        Invoke("Preparation", _shotRate-1);
        Invoke("CanShootAgain", _shotRate);
        if (_shotRate == 2) AkSoundEngine.PostEvent("Laser_loading2", gameObject);
        if (_shotRate == 4) AkSoundEngine.PostEvent("Laser_loading4", gameObject);
    }

    protected void Preparation()
    {
        _laserShot.PreActiveMode();
    }

    protected virtual void CanShootAgain()
    {
        _canShoot = true;
    }

    public override void SetModeFree()
    {
        _laserShot.DesactiveMode();
        base.SetModeFree();
    }

}
