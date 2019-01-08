using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Module : MonoBehaviour
{
    //Data
    [SerializeField]
    public float _weight = 1f;

    [SerializeField]
    private float _freeRotationSpeed = 0.7f;

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
        //TO DO : Go down at the speed of the scrolling + disapear after a certain point
        transform.Rotate(Vector3.forward * _freeRotationSpeed);
    }

    protected void Update()
    {
        if (moduleAction != null) moduleAction();
    }

    public virtual void SetDeathMode()
    {
        moduleAction = DoActionVoid;
        transform.SetParent(null);
        Destroy(this.gameObject);
    }

}
