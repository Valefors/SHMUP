using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : ModuleUser
{
    private static string _VERTICAL_AXIS = "Vertical";
    private static string _HORIZONTAL_AXIS = "Horizontal";

    // Update is called once per frame
    void Update()
    {
        float lXmovValue = Input.GetAxis(_HORIZONTAL_AXIS);
        float lYmovValue = Input.GetAxis(_VERTICAL_AXIS);

        if (lXmovValue != 0 || lYmovValue != 0)
        {
            Move(lXmovValue, lYmovValue);
        }

        if (Input.GetAxisRaw("Fire1") != 0)
        {
            ManageShoot();
        }
    }

    void Move(float lXmovValue, float lYmovValue)
    {
        Vector3 lMovement = new Vector3(lXmovValue, lYmovValue, 0);
        lMovement = lMovement.normalized * _speed * Time.deltaTime;

        _transform.Translate(lMovement);
    }




    #region Abstract

    protected override bool Ally()
    {
        return true;
    }

    protected override Vector3 ShotDirection()
    {
        return Vector3.up;
    }

    #endregion

}
