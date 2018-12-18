using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Personal Datas")]
    [SerializeField]
    protected Transform _spawnShot;
    protected Transform _transform;
    [SerializeField]
    protected GameObject _prefabShot;

    private bool _canShoot = true;

    private static string _VERTICAL_AXIS = "Vertical";
    private static string _HORIZONTAL_AXIS = "Horizontal";

    [Header("Gameplay Datas")]
    [SerializeField]
    protected float _speed;
    [SerializeField]
    protected float _shotRate = 0.1f;

    [SerializeField]
    private List<Module> _modulesList = new List<Module>();
    private int _listLenght = 0;

    // Start is called before the first frame update
    protected void Start()
    {
        _transform = this.transform;
        _listLenght = _modulesList.Count;
    }

    // Update is called once per frame
    void Update()
    {
        float lXmovValue = Input.GetAxis(_HORIZONTAL_AXIS);
        float lYmovValue = Input.GetAxis(_VERTICAL_AXIS);

        SetModuleVoidMode();

        if (lXmovValue != 0 || lYmovValue != 0)
        {
            Move(lXmovValue, lYmovValue);
        }

        if (Input.GetAxisRaw("Fire1") != 0)
        {
            SetModuleActionMode();
        }
    }

    void Move(float lXmovValue, float lYmovValue)
    {
        Vector3 lMovement = new Vector3(lXmovValue, lYmovValue, 0);
        lMovement = lMovement.normalized * _speed * Time.deltaTime;

        _transform.Translate(lMovement);
    }

    void SetModuleVoidMode()
    {
        for (int i = 0; i < _listLenght; i++)
        {
            _modulesList[i].SetModeVoid();
        }
    }

    void SetModuleActionMode()
    {
        for (int i = 0; i < _listLenght; i++)
        {
            _modulesList[i].SetModeNormal();
        }
    }

    #region Shoot_region

    /*protected virtual void ManageShoot()
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
    }*/

    #endregion

    #region GetDamage
    //TO-DO: INVERSER, PLAYER DOIT CHECKER LUI LES COLLISIONS ET PAS LES BULLETS
    public virtual void GetHit()
    {
        Debug.Log("Player get hit !");
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D pCol)
    {
        if (pCol.GetComponent<Module>())
        {
            Module lModule = pCol.GetComponent<Module>();
            if (lModule.GetComponent<Canon>() != null) lModule.GetComponent<Canon>().isEnemy = false;
            lModule.transform.parent = _transform;

            _modulesList.Add(lModule);
            
            _listLenght++;
        }
    }
}
