using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Module : MonoBehaviour
{
    //Fonction de l'état du cube
    public delegate void DelAction();
    DelAction moduleAction;

    protected virtual void OnEnable()
    {
        SetModeNormal();
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

}
