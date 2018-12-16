using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ModuleUser : MonoBehaviour
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
    [SerializeField]
    protected float _shotRate = 0.1f;



    private bool _canShoot = true;


    // Start is called before the first frame update
    void Start()
    {
        AccessibleStart();
    }

    // Start is called before the first frame update
    protected virtual void AccessibleStart()
    {
        _transform = this.transform;
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
        shotShot.SetUp(!Ally(), ShotDirection());
        _canShoot = false;
        Invoke("CanShootAgain", _shotRate);
    }

    protected virtual void CanShootAgain()
    {
        _canShoot = true;
    }

    #endregion

    protected abstract bool Ally();
    protected abstract Vector3 ShotDirection();

    #region GetDamage
    public virtual void GetHit()
    {
        Debug.Log("I'm getting hit ! Outch !");
        //pv--;
        //Death();
    }

    #endregion

}
