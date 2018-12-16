using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    
    [Header("Personal Datas")]
    private int pv = 1;
    private float _shotRate = 0.2f;
    [SerializeField]
    private Transform _spawnShot;
    private Transform _transform;
    [SerializeField]
    private GameObject _prefabShot;

    private bool _canShoot = true;

    private void Update()
    {
        ManageShoot();
    }

    void ManageShoot()
    {
        if (_canShoot)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Shot shotShot = Instantiate(_prefabShot, _spawnShot.position, Quaternion.identity).GetComponent<Shot>();
        shotShot.SetUp(true, Vector3.down);
        _canShoot = false;
        Invoke("CanShootAgain", _shotRate);
    }

    void CanShootAgain()
    {
        _canShoot = true;
    }


    public void GetHit()
    {
        pv--;
        if(pv <= 0)
            Death();
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }



}
