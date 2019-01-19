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
    [SerializeField] MeshRenderer sail1;
    [SerializeField] MeshRenderer sail2;
    [SerializeField] MeshRenderer sail3;

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
    private Color baseColor;
    private Color sailColor;
    private float maxHP;
    private float div;


    private void OnEnable()
    {
        maxHP = _pv;
        sailColor = new Color(sail1.material.color.r, sail1.material.color.g, sail1.material.color.b, 1);
        baseColor = new Color(sailColor.r, sailColor.g, sailColor.b, 1);
        if(maxHP <=3)
        {
            baseColor.g = 0.8f;
            div = 3;
        }
        else if(maxHP <=6)
        {
            baseColor.g = 0.7f;
            div = 6;
        }
        else if (maxHP <= 12)
        {
            baseColor.g = 0.6f;
            div = 12;
        }
        else if (maxHP <= 18)
        {
            baseColor.g = 0.5f;
            baseColor.b = 0.6f;
            div = 18;
        }
        else if (maxHP <= 30)
        {
            baseColor.g = 0.4f;
            baseColor.b = 0.5f;
            div = 30;
        }
        else if (maxHP <= 50)
        {
            baseColor.g = 0.2f;
            baseColor.b = 0.3f;
            div = 50;
        }
        else if (maxHP <= 200)
        {
            baseColor.g = 0f;
            baseColor.b = 0f;
            baseColor.r = 0;
            div = 200;
        }


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

    public int GetPv()
    {
        return _pv;
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

        _pF = null;

        GetComponentInChildren<Animator>().SetTrigger("Death");

        FindObjectOfType<Player>().DefeatedBoss();
        //Stop shooting. Move to a point (fixe);
        //+ call Player.DefeatedBoss();
        //Screen goes white

        //Wait a while then 
        StartCoroutine(WaitForEndBossDeathAnimation());
    }

    IEnumerator GoToPoint(Vector3 pointToGo, float timeToDoIt, float delay)
    {
        yield return new WaitForSeconds(delay);
        float lLerp = 0;
        Vector3 startPos = _transform.position;
        while (lLerp < 1)
        {
            lLerp += Time.deltaTime / timeToDoIt;
            _transform.position = Vector3.Lerp(startPos, pointToGo, lLerp);
            yield return new WaitForSeconds(0.01f);
        }
        _transform.position = pointToGo;
    }


    IEnumerator WaitForEndBossDeathAnimation()
    {
        yield return new WaitForSeconds(8f);

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
        LifeSail();
        Move();

    }

    private void LifeSail()
    {
        Debug.Log("couleur:" + sailColor);
        Debug.Log("div:" + div);
        sailColor.g = (baseColor.g + ((maxHP- _pv) / div));
        sailColor.b = (baseColor.b + ((maxHP - _pv) / div));
        if(maxHP==200)sailColor.r = (baseColor.r + ((maxHP - _pv) / div));

        sail1.material.SetColor("_Color", sailColor);
        sail2.material.SetColor("_Color", sailColor);
        sail3.material.SetColor("_Color", sailColor);
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
