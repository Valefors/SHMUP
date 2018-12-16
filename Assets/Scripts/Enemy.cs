using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int pv = 1;

    public void GetHit()
    {
        pv--;
        Death();
    }

    private void Death()
    {
        Destroy(this.gameObject);
    }

}
