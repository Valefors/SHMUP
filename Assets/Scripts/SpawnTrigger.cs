using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] _wavesArray;
    bool _isActive = false;

    private void OnTriggerEnter2D(Collider2D pCol)
    {
        if (pCol.gameObject.tag == "Player" && !_isActive)
        {
            LaunchWaves();
            _isActive = true;
        }
    }

    void LaunchWaves()
    {
        for (int i = 0; i < _wavesArray.Length; i++)
        {
            Instantiate(_wavesArray[i]);
        }
    }
}
