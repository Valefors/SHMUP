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
    [SerializeField] protected bool useSalve = false;
    [SerializeField] protected int _numberShotSalve;
    [SerializeField] protected float _timeWaitSalve;

    [SerializeField] protected GameObject _prefabShot;

    private int countedShots=0;
    private float waitingTime;
    private bool isWaiting=false;
    private bool isShooting=true;

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
        if (!useSalve || ((isWaiting && waitingTime > _timeWaitSalve) || isShooting))
        {
            Shot lShot = Instantiate(_prefabShot, _transform.position, Quaternion.identity).GetComponent<Shot>();
            lShot.SetUp(isEnemy, _transform.rotation, GetSpeed(), _hitValue);
            _canShoot = false;

            if (!isEnemy)
            {
                AkSoundEngine.PostEvent("Shot", gameObject);
            }
            Invoke("CanShootAgain", _shotRate);

            if(useSalve)
            {
                countedShots++;
                if (isWaiting)
                {
                    waitingTime = 0;
                    isWaiting = false;
                    isShooting = true;
                }
                if (countedShots >= _numberShotSalve && isShooting)
                {
                    countedShots = 0;
                    isShooting = false;
                    isWaiting = true;
                }
            }
        }
        else waitingTime += Time.deltaTime;
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

