using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardInstance : MonoBehaviour
{
    private static LeaderboardInstance _manager;
    public static LeaderboardInstance manager {
        get {
            return _manager;
        }
    }

    private void Awake()
    {
        if (_manager == null) _manager = this;

        else if (_manager != this) Destroy(gameObject);
    }
}
