using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour
{
    private static GameObject _instance;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
}
