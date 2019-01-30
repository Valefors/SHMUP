using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossTrigger : MonoBehaviour
{
    Enemy e;
    private void Start()
    {
        //GameManager.manager.BossHealthBar = FindObjectOfType<Canvas>().GetComponent<Slider>();
        e = GetComponent<Enemy>();
        GameManager.manager.EnableBossHealthBar(e.GetPv());
    }

    private void Update()
    {
       if(GameManager.manager.hasFilled)GameManager.manager.UpdateBossHealth(e.GetPv());
    }
}
