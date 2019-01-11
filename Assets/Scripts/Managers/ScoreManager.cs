using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [HideInInspector] public float score {
        get { return _score; }
    }

    private float _score = 0;

    private static ScoreManager _manager;
    public static ScoreManager manager {
        get { return _manager; }
    }

    private void Awake()
    {
        if (_manager == null) _manager = this;

        else if (_manager != this) Destroy(gameObject);
    }

    public void UpdateScore(float pValue)
    {
        _score += pValue;
        print(score);
    }
}
