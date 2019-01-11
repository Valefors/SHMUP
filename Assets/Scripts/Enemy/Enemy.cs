using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Personal Datas")]
    protected Transform _transform;
    PathFollower _pF;
    Rigidbody2D _rb;
    [SerializeField] Scrap _scrap;

    [Header("Gameplay Datas")]
    [SerializeField] protected float _speed;
    [SerializeField] protected bool _moveLoop = false;
    [SerializeField] int _pv = 1;

    [SerializeField] private Module[] _modulesList;
    private int _listLenght = 0;

    [SerializeField] [Range(0,1)] protected float _dropLoot = 0.5f;

    private void OnEnable()
    {
        _transform = this.transform;

        _rb = GetComponent<Rigidbody2D>();
        _pF = GetComponentInChildren<PathFollower>();
        if (_pF == null) Debug.Log("PATH FOLLOWER MISSING IN " + transform.name);
    }

    private void Start()
    {
        FillModuleArray();
        SetModuleActionMode();
        EventManager.StartListening(EventManager.GAME_OVER_EVENT, GameOver);
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

        else
        {
            if (_pF.currentNode + 1 == _pF.nodesPosition.Count && !_moveLoop) return;
            _pF.currentNode = (_pF.currentNode + 1) % _pF.nodesPosition.Count;     
        }

        // TEST LD AXEL
        Quaternion saved = _pF.nodesRotation[_pF.currentNode];
        _transform.rotation = Quaternion.Lerp(_transform.rotation, saved, 0.05f);
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
        float randomValue = Random.Range(0f,1f);

        if (randomValue < _dropLoot)
            DropItem();
        Destroy(this.gameObject);
    }

    void DropItem()
    {
        //Just a try
        //TO DO FACTORY
        if (_listLenght != 0)
        {
 
            int randomIndex = Random.Range(0, _modulesList.Length);

            Module lModule = _modulesList[randomIndex];

            lModule.transform.SetParent(null); //put it in a container of all "free" module who will go down , maybe ?
            lModule.SetModeFree();
            lModule.free = true;
            //TO DO : a launch fonction to launch them in a direction
        }
    }

    private void Update()
    {
        if (GameManager.manager.isPause) return;
        Move();
    }

    void GameOver()
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventManager.GAME_OVER_EVENT, GameOver);
    }
}
