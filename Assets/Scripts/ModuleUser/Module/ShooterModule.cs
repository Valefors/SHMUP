using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShooterModule : Module
{
    public bool isEnemy = true;

    [Header("Personal Datas")]
    protected Transform _transform;
    protected bool _canShoot = true;
}
