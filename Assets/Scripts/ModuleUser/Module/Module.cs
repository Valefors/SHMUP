using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Module : MonoBehaviour
{
    //Data
    [SerializeField]
    public float _weight = 1f;

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
