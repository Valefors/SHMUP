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
    [Range(-1, 1)]
    private float _horizontalAccDecLerpValue;
    private Vector3 _horizontalLastMovement = Vector3.zero;

    [HideInInspector] [SerializeField] private AnimationCurve _verticalAccelerationCurve;
    [HideInInspector] [SerializeField] private AnimationCurve _verticalDecelerationCurve;
    [HideInInspector] [SerializeField] private float _verticalAccSpeed = 1;
    [HideInInspector] [SerializeField] private float _verticalDecSpeed = 1;
    [Range(-1, 1)]
    private float _verticalAccDecLerpValue;
    private Vector3 _verticalLastMovement = Vector3.zero;
    
    [SerializeField]
    private AnimationCurve _swingCurve;
    [SerializeField]
    private float _swingDegree;

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
        if (GameManager.manager.isPause) return;

        SetModuleVoidMode();

        Move();

        if (Input.GetAxisRaw("Fire1") != 0)
        {
            SetModuleActionMode();
        }
    }

    #region Movement

    void Move()
    {
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
    }

    void HorizontalMove(float lXmovValue)
    {
        _horizontalAccDecLerpValue += Time.deltaTime * _horizontalAccSpeed * Mathf.Sign(lXmovValue);
        _horizontalAccDecLerpValue = Mathf.Clamp(_horizontalAccDecLerpValue, -1, 1);

        Vector3 lMovement = new Vector3(Mathf.Abs(lXmovValue), 0, 0);
        float lSpeed = _speedWeightCurve.Evaluate(_weight) * _speed;
        lMovement = lMovement.normalized * lSpeed * Time.deltaTime * Mathf.Sign(_horizontalAccDecLerpValue);
        
        _horizontalLastMovement = lMovement;

        lMovement *= _horizontalAccelerationCurve.Evaluate(Mathf.Abs(_horizontalAccDecLerpValue));
        print(lMovement.x + "This ois here: " + _horizontalLastMovement.x + " and lerp = " + _horizontalAccelerationCurve.Evaluate(Mathf.Abs(_horizontalAccDecLerpValue)) + " lerp = " + _horizontalAccDecLerpValue + " (calculus = " + lMovement.normalized * lSpeed + ", " + Time.deltaTime + " , " + Mathf.Sign(_horizontalAccDecLerpValue));

        _transform.Translate(lMovement, Space.World);
        ChangeRotation();
    }
    
    void VerticalMove(float lYmovValue)
    {
        _verticalAccDecLerpValue += Time.deltaTime * _verticalAccSpeed * Mathf.Sign(lYmovValue);
        _verticalAccDecLerpValue = Mathf.Clamp(_verticalAccDecLerpValue, -1, 1);

        Vector3 lMovement = new Vector3(0, Mathf.Abs(lYmovValue), 0);
        float lSpeed = _speedWeightCurve.Evaluate(_weight) * _speed;
        lMovement = lMovement.normalized * lSpeed * Time.deltaTime * Mathf.Sign(_verticalAccDecLerpValue);

        _verticalLastMovement = lMovement;

        lMovement *= _verticalAccelerationCurve.Evaluate(Mathf.Abs(_verticalAccDecLerpValue));

        _transform.Translate(lMovement, Space.World);
    }

    void HorizontalSlowDown()
    {
        float pastLerp = _horizontalAccDecLerpValue;
        _horizontalAccDecLerpValue -= Time.deltaTime * _horizontalDecSpeed * Mathf.Sign(_horizontalAccDecLerpValue);
        if(Mathf.Sign(pastLerp) != Mathf.Sign(_horizontalAccDecLerpValue))
        {
            _horizontalAccDecLerpValue = 0;
        }

        Vector3 lMovement = _horizontalLastMovement;
        lMovement *= _horizontalDecelerationCurve.Evaluate(Mathf.Abs(_horizontalAccDecLerpValue));
        print("SLOW = " + lMovement.x + " lastMovement = " + _horizontalLastMovement.x + " and lerp = " + _horizontalDecelerationCurve.Evaluate(Mathf.Abs(_horizontalAccDecLerpValue)) + " lerp = " + _horizontalAccDecLerpValue);

        _transform.Translate(lMovement, Space.World);
        ChangeRotation();
    }

    void VerticalSlowDown()
    {
        float pastLerp = _verticalAccDecLerpValue;
        _verticalAccDecLerpValue -= Time.deltaTime * _verticalDecSpeed * Mathf.Sign(_verticalAccDecLerpValue);
        if (Mathf.Sign(pastLerp) != Mathf.Sign(_verticalAccDecLerpValue))
        {
            _verticalAccDecLerpValue = 0;
        }

        Vector3 lMovement = _verticalLastMovement;
        lMovement *= _verticalDecelerationCurve.Evaluate(Mathf.Abs(_verticalAccDecLerpValue));

        _transform.Translate(lMovement, Space.World);
    }
    
    void ChangeRotation()
    {
        float lSwingValue =_horizontalAccDecLerpValue;

        Vector3 lEuler = _transform.rotation.eulerAngles;
        lEuler.z = _swingCurve.Evaluate(lSwingValue);
        _transform.rotation = Quaternion.Euler(lEuler);
    }

    #endregion


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
            if(!GameManager.manager.isLD) EventManager.TriggerEvent(EventManager.GAME_OVER_EVENT);
            Debug.Log("This is a gameOver");
        }
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D pCol)
    {
        if (GameManager.manager.isPause) return;

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
