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

    [Header("Visual info")]
    [SerializeField]
    private GameObject _explosionWhenDestroyed;
    [SerializeField]
    private Collider2D[] _colliderOfTheModule;

    public bool free = false;

    //Fonction de l'état du cube
    public delegate void DelAction();
    DelAction moduleAction;

    protected virtual void OnEnable()
    {
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
        if (GameManager.manager.isPause) return;

        if (moduleAction != null) moduleAction();
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
    

}
