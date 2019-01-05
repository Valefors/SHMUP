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
    private AnimationCurve _speedWeightCurve;
    private float _weight = 0f;

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

        UpdateWeight();
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
        float lSpeed = _speedWeightCurve.Evaluate(_weight) * _speed;
        lMovement = lMovement.normalized * lSpeed * Time.deltaTime;

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

    #region GetDamage
    public virtual void GetHit()
    {
        Shaker.instance.Shake();

        if (_listLenght != 1)
        {
            RemoveLastModule();
        }
        else
        {
            Debug.Log("This is a gameOver");
        }
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D pCol)
    {
        if (pCol.GetComponent<Module>())
        {
            AddModule(pCol.GetComponent<Module>());
        }
    }

    private void AddModule(Module module)
    {
        if (module.GetComponent<Canon>() != null) module.GetComponent<Canon>().isEnemy = false;

        module.transform.parent = _transform;

        _modulesList.Add(module);
        _listLenght++;

        UpdateWeight();
    }

    private void RemoveLastModule()
    {
        Module lModuleToDestroy = _modulesList[_listLenght - 1];
        _modulesList.RemoveAt(_listLenght - 1);
        _listLenght--;
        lModuleToDestroy.SetDeathMode();

        UpdateWeight();
    }

    private void UpdateWeight()
    {
        float lNewWeight = 0;
        foreach (Module lModule in _modulesList)
        {
            lNewWeight += lModule._weight;
        }
        _weight = lNewWeight;
    }

}
