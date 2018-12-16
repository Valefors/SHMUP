using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Personal Datas")]
    [SerializeField]
    protected Transform _spawnShot;
    protected Transform _transform;
    [SerializeField]
    protected GameObject _prefabShot;


    [Header("Gameplay Datas")]
    [SerializeField]
    protected float _speed;
    private int _pv = 1;//PV ? List module plutôt ?
    [SerializeField]
    protected float _shotRate = 0.1f;

    private bool _canShoot = true;

    private void Update()
    {
        ManageShoot();
    }

    #region Shoot_region
    protected virtual void ManageShoot()
    {
        if (_canShoot)
        {
            Shoot();
        }
    }

    protected virtual void Shoot()
    {
        Shot shotShot = Instantiate(_prefabShot, _spawnShot.position, Quaternion.identity).GetComponent<Shot>();
        shotShot.SetUp(false, Vector3.up);
        _canShoot = false;
        Invoke("CanShootAgain", _shotRate);
    }

    protected virtual void CanShootAgain()
    {
        _canShoot = true;
    }

    #endregion

    #region GetDamage
    public virtual void GetHit()
    {
        _pv--;
        if (_pv <= 0)
            Death();
    }

    #endregion

    private void Death()
    {
        Destroy(this.gameObject);
    }



}
