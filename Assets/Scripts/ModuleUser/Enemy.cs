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

    [SerializeField]
    private List<Module> _modulesList = new List<Module>();
    private int _listLenght = 0;

    private void OnEnable()
    {
        _listLenght = _modulesList.Count;
        _transform = this.transform;
    }

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
        //TO DO FACTORY
        Module lModule = Instantiate(_modulesList[0], _transform.position, _transform.rotation);
        lModule.SetModeVoid();

        Destroy(this.gameObject);
    }



}
