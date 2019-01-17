using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Canvas : MonoBehaviour
{
    private static Canvas _instance;

    private void Start()
    {
        if (_instance == null) _instance = this;

        else if (_instance != this) Destroy(gameObject);
    }
}
