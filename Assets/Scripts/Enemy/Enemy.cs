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
    [SerializeField] GameObject _explosionWhenHit;
    [SerializeField] GameObject _explosionWhenDead;
    [SerializeField] Light _ownLight;

    [Header("Gameplay Datas")]
    [SerializeField] protected float _speed;
    [SerializeField] protected bool _moveLoop = false;
    [SerializeField] int _pv = 1;
    [SerializeField] protected float rotationSpeed;
    [SerializeField] protected bool _boss = false;

    [SerializeField] private Module[] _modulesList;
    private int _listLenght = 0;

    [Header("Score")]
    [SerializeField] private float _scoreValue = 0f;

    [SerializeField] [Range(0,1)] protected float _dropLoot = 0.5f;

    private float timeSpent;
    

    private void OnEnable()
    {
        GameManager.manager.enemiesAlive++;
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

            if (timeSpent >= _pF.nodesWaitTime[_pF.currentNode])
            {
                _pF.currentNode = (_pF.currentNode + 1) % _pF.nodesPosition.Count;
                timeSpent = 0;
            }
            else timeSpent += Time.deltaTime;
        }

        Quaternion saved = _pF.nodesRotation[_pF.currentNode];
        _transform.rotation = Quaternion.Lerp(_transform.rotation, saved, Time.deltaTime * rotationSpeed);
    }

    #region GetDamage
    public virtual void GetHit(int pHitValue, Vector3 impactPosition)
    {
        _pv = _pv - pHitValue;



        if (_pv <= 0)
            Death();
       // else
       // {
            CreateParticleDamage(impactPosition);
       // }
    }

    void CreateParticleDamage(Vector3 impactPosition)
    {
        Instantiate(_explosionWhenHit, impactPosition, Quaternion.identity, null);
        //destroy automatic on the explosion
    }

    void CreateParticleDeath(Vector3 impactPosition)
    {
        Instantiate(_explosionWhenDead, impactPosition, Quaternion.identity, null);
        GameManager.manager.PauseFeel();
        //destroy automatic on the explosion
    }

    #endregion

    private void Death()
    {
        if (_boss)
        {
            BossDeath();
        }
        else
        {
            float randomValue = Random.Range(0f, 1f);

            if (randomValue < _dropLoot)
                DropItem();

            TrueDeath();
        }
    }

    void TrueDeath()
    {
        AkSoundEngine.PostEvent("Kill", gameObject);
        ScoreManager.manager.UpdateScore(_scoreValue);
        CreateParticleDeath(_transform.position);

        if (_ownLight != null)
        {
            _ownLight.transform.SetParent(GameManager.manager.scrolling);
            Destroy(_ownLight.gameObject, 3);
        }

        GameManager.manager.enemiesAlive--;

        Destroy(this.gameObject);
    }

    void BossDeath()
    {
        Shaker.instance.FinalShake();

        GetComponentInChildren<Animator>().SetTrigger("Death");

        //Stop shooting. Move to a point (fixe);
        //+ call Player.DefeatedBoss();
        //Screen goes white

        //Wait a while then 
        StartCoroutine(WaitForEndBossDeathAnimation());
    }

    IEnumerator WaitForEndBossDeathAnimation()
    {
        yield return new WaitForSeconds(5f);

        if (!GameManager.manager.isLD) EventManager.TriggerEvent(EventManager.GAME_OVER_EVENT);

        TrueDeath();
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
        if (!GameManager.manager.isPlaying) return;
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
