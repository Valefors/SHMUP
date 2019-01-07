using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Personal Datas")]
    protected Transform _transform;

    private static string _VERTICAL_AXIS = "Vertical";
    private static string _HORIZONTAL_AXIS = "Horizontal";

    [Header("Gameplay Datas")]
    [SerializeField]
    protected float _shotRate = 0.1f;

    [SerializeField]
    private List<Module> _modulesList = new List<Module>();
    private int _listLenght = 0;

    [Header("Movement Part")]
    [SerializeField]
    private float _speed;
    [SerializeField]
    private AnimationCurve _speedWeightCurve;
    private float _weight = 0f;

    [HideInInspector] [SerializeField] private AnimationCurve _horizontalAccelerationCurve;
    [HideInInspector] [SerializeField] private AnimationCurve _horizontalDecelerationCurve;
    [HideInInspector] [SerializeField] private float _horizontalAccSpeed = 1;
    [HideInInspector] [SerializeField] private float _horizontalDecSpeed = 1;
    [Range(0, 1)]
    private float _horizontalAccDecLerpValue;
    private Vector3 _horizontalLastMovement = Vector3.zero;

    [HideInInspector] [SerializeField] private AnimationCurve _verticalAccelerationCurve;
    [HideInInspector] [SerializeField] private AnimationCurve _verticalDecelerationCurve;
    [HideInInspector] [SerializeField] private float _verticalAccSpeed = 1;
    [HideInInspector] [SerializeField] private float _verticalDecSpeed = 1;
    [Range(0, 1)]
    private float _verticalAccDecLerpValue;
    private Vector3 _verticalLastMovement = Vector3.zero;

    // Start is called before the first frame update
    private void Start()
    {
        _transform = this.transform;
        _listLenght = _modulesList.Count;

        UpdateWeight();
    }

    // Update is called once per frame
    void Update()
    {
        SetModuleVoidMode();

        float lXmovValue = Input.GetAxisRaw(_HORIZONTAL_AXIS);
        if (lXmovValue != 0)
            HorizontalMove(lXmovValue);
        else if (_horizontalAccDecLerpValue != 0)
            HorizontalSlowDown();

        float lYmovValue = Input.GetAxisRaw(_VERTICAL_AXIS);
        if (lYmovValue != 0)
            VerticalMove(lYmovValue);
        else if (_verticalAccDecLerpValue != 0)
            VerticalSlowDown();

        if (Input.GetAxisRaw("Fire1") != 0)
        {
            SetModuleActionMode();
        }
    }

    void HorizontalMove(float lXmovValue)
    {
        if (_horizontalAccDecLerpValue != 1)
        {
            _horizontalAccDecLerpValue += Time.deltaTime * _horizontalAccSpeed;
            _horizontalAccDecLerpValue = Mathf.Clamp01(_horizontalAccDecLerpValue);
        }
        Vector3 lMovement = new Vector3(lXmovValue, 0, 0);
        float lSpeed = _speedWeightCurve.Evaluate(_weight) * _speed;
        lMovement = lMovement.normalized * lSpeed * Time.deltaTime;

        _horizontalLastMovement = lMovement;

        lMovement *= _horizontalAccelerationCurve.Evaluate(_horizontalAccDecLerpValue);

        _transform.Translate(lMovement);
    }

    void VerticalMove(float lYmovValue)
    {
        if (_verticalAccDecLerpValue != 1)
        {
            _verticalAccDecLerpValue += Time.deltaTime * _verticalAccSpeed;
            _verticalAccDecLerpValue = Mathf.Clamp01(_verticalAccDecLerpValue);
        }
        Vector3 lMovement = new Vector3(0, lYmovValue, 0);
        float lSpeed = _speedWeightCurve.Evaluate(_weight) * _speed;
        lMovement = lMovement.normalized * lSpeed * Time.deltaTime;

        _verticalLastMovement = lMovement;

        lMovement *= _verticalAccelerationCurve.Evaluate(_verticalAccDecLerpValue);

        _transform.Translate(lMovement);
    }

    void HorizontalSlowDown()
    {
        _horizontalAccDecLerpValue -= Time.deltaTime * _horizontalDecSpeed;
        _horizontalAccDecLerpValue = Mathf.Clamp01(_horizontalAccDecLerpValue);

        Vector3 lMovement = _horizontalLastMovement;
        lMovement *= _horizontalDecelerationCurve.Evaluate(_horizontalAccDecLerpValue);

        _transform.Translate(lMovement);
    }

    void VerticalSlowDown()
    {
        _verticalAccDecLerpValue -= Time.deltaTime * _verticalDecSpeed;
        _verticalAccDecLerpValue = Mathf.Clamp01(_verticalAccDecLerpValue);

        Vector3 lMovement = _verticalLastMovement;
        lMovement *= _verticalDecelerationCurve.Evaluate(_verticalAccDecLerpValue);

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
        Module moduleCollided = pCol.GetComponent<Module>();
        if (moduleCollided != null)
        {
            if (moduleCollided.free) //need to know if there parent are still enemy (or even friend)
                AddModule(moduleCollided);
        }
    }

    private void AddModule(Module module)
    {
        if (module.GetComponent<Canon>() != null) module.GetComponent<Canon>().isEnemy = false;

        module.transform.parent = _transform;

        Vector3 directionToLookAt = module.transform.position - _transform.position;
        //TO DO : need to make a clear feedback
        //something to make the module really go from start direction to this one
        module.transform.rotation = Quaternion.LookRotation(Vector3.forward, directionToLookAt);
        module.free = false;

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
