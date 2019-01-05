using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Personal Datas")]
    protected Transform _transform;
    PathFollower _pF;
    Rigidbody2D _rb;

    [Header("Gameplay Datas")]
    [SerializeField]
    protected float _speed;
    private int _pv = 1;
    //Shot rate currently on module
    /*[SerializeField]
    protected float _shotRate = 0.1f;*/

    [SerializeField]
    private Module[] _modulesList;
    private int _listLenght = 0;

    private void OnEnable()
    {
        FillModuleArray();
        SetModuleActionMode();

        _transform = this.transform;

        _rb = GetComponent<Rigidbody2D>();
        _pF = GetComponentInChildren<PathFollower>();
        if (_pF == null) Debug.Log("PATH FOLLOWER MISSING IN " + transform.name);
    }

    void FillModuleArray()
    {
        _modulesList = GetComponentsInChildren<Module>();
        _listLenght = _modulesList.Length;
    }

    void SetModuleActionMode()
    {
        for (int i = 0; i < _listLenght; i++)
        {
            _modulesList[i].SetModeNormal();
        }
    }

    void Move()
    {
        if (_pF == null) return;

        if (_transform.position != _pF.nodesPosition[_pF.currentNode])
        {
            Vector3 pos = Vector3.MoveTowards(_transform.position, _pF.nodesPosition[_pF.currentNode], _speed * Time.deltaTime);
            _rb.MovePosition(pos);
        }

        else _pF.currentNode = (_pF.currentNode + 1) % _pF.nodesPosition.Count;
    }

    #region GetDamage
    public virtual void GetHit(int pHitValue)
    {
        _pv = _pv - pHitValue;
        if (_pv <= 0)
            Death();
    }

    #endregion

    private void Death()
    {
        //Just a try
        //TO DO FACTORY
        if(_listLenght != 0)
        {
            Module lModule = _modulesList[0];
            lModule.transform.SetParent(null); //put it in a container of all "free" module who will go down , maybe ?
            lModule.SetModeVoid();
            //TO DO : a launch fonction to launch them in a direction
        }

        Destroy(this.gameObject);
    }

    private void Update()
    {
        Move();
    }
}
