using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Module : MonoBehaviour
{
    //Data
    [SerializeField]
    public float _weight = 1f;

    [SerializeField]
    private float _freeSpeedFactor = 1f;
    [SerializeField]
    private float _freeRotationSpeed = 0.7f;
    private Vector3 _scrollingVector = Vector3.zero;

    [SerializeField]
    public bool rotateWhenPickUp = true;

    [Header("Visual info")]
    [SerializeField]
    private GameObject _explosionWhenDestroyed;
    [SerializeField]
    private Animator _animator;
    [SerializeField]
    private GameObject _canonObject;
    [SerializeField]
    private Material _freeLight;
    [SerializeField]
    private Material _freeRedLight;

    public bool free = false;

    private MeshRenderer _lightMesh;
    private Color freeColor;
    private Color _saveSecLight;
    private Color _saveMainLight;
    private Material _mainLightMaterial;
    private Material _secLightMaterial;
    private int mainI;
    private int secI;

    protected int moduleType=-1;


    public delegate void DelAction();
    DelAction moduleAction;

    protected virtual void OnEnable()
    {
        _lightMesh = _canonObject.GetComponent<MeshRenderer>();
        Material[] tempList = _lightMesh.materials;
        Debug.Log("Taille des matériaux:" + tempList.Length);
        
        mainI = 3;
        switch(moduleType)
        {
            case 1:
                mainI = 3;
                secI = 0;
                break;
            case 2:
                secI = 1;
                break;
            case 3:
                secI = 2;
                break;
            case 4:
                secI = 1;
                break;
        };
        _mainLightMaterial = tempList[mainI];
        _secLightMaterial = tempList[secI];
        _saveMainLight = _mainLightMaterial.GetColor("_EmissionColor");
        _saveSecLight = _secLightMaterial.GetColor("_EmissionColor");
        SetModeVoid();
    }

    public virtual void SetModeVoid()
    {
        moduleAction = DoActionVoid;
    }

    public virtual void DoActionVoid()
    {
        return;
    }

    public virtual void SetModeNormal()
    {
        moduleAction = DoActionNormal;
    }
    
    public abstract void DoActionNormal();

    public virtual void SetModeFree()
    {
        moduleAction = DoActionFree;
    }

    public virtual void DoActionFree()
    {
        if(_scrollingVector == Vector3.zero)
        {
            SetScrollingVector();
        }

        transform.Translate(_scrollingVector * Time.deltaTime, Space.World);
        transform.Rotate(Vector3.forward * _freeRotationSpeed * Time.deltaTime);
    }

    protected virtual void SetScrollingVector()
    {
        //TO DO : Changing that to a more "error proof" method
        _scrollingVector = GameManager.manager.scrollingVector;
        _scrollingVector *= _freeSpeedFactor;
    }

    protected void Update()
    {
        if (!GameManager.manager.isPlaying) return;

        if (moduleAction != null) moduleAction();
        if (transform.position.y < -50) Destroy(gameObject);

        if(free)
        {
            //_mainLightMaterial.EnableKeyword("_EMISSION");
            _mainLightMaterial.SetColor("_EmissionColor", _mainLightMaterial.color* 1.5f);
            _mainLightMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;

            _secLightMaterial.SetColor("_EmissionColor", _secLightMaterial.color* 1.4f);
            _secLightMaterial.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;
        }
       else
        {
            _mainLightMaterial.SetColor("_EmissionColor", _saveMainLight);
            _secLightMaterial.SetColor("_EmissionColor", _saveSecLight);
        }
    }

    public virtual void SetDeathMode()
    {
        moduleAction = DoActionVoid;
        CreateParticleDamage();
        transform.SetParent(null);
        Destroy(this.gameObject);
    }

    void CreateParticleDamage()
    {
        Instantiate(_explosionWhenDestroyed, transform.position, Quaternion.identity, null);
        //destroy automatic on the explosion
    }

    public void ModuleClignote(bool turnOn)
    {
        _animator.SetBool("Invulnerability", turnOn);
    }

}
