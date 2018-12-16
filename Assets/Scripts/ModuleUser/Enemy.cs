using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : ModuleUser
{
    [Header("Gameplay Datas")]
    private int pv = 1;//PV ? List module plutôt ?

    private void Update()
    {
        ManageShoot();
    }

    public override void GetHit()
    {
        pv--;
        if(pv <= 0)
            Death();
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

    #region Abstract

    protected override bool Ally()
    {
        return false;
    }

    protected override Vector3 ShotDirection()
    {
        return Vector3.down;
    }

    #endregion

}
