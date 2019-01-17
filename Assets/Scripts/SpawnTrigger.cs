using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] _wavesArray;
    [SerializeField] bool _isSkippable;
    [SerializeField] int _numberOfEnemiesMax;
    bool _isActive = false;

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        if (transform.GetComponent<BoxCollider2D>() != null)
            Gizmos.DrawWireCube(transform.position, transform.GetComponent<BoxCollider2D>().bounds.size);
    }

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
            if (!_isSkippable || CanLaunch()) Instantiate(_wavesArray[i]);
        }
    }

    bool CanLaunch()
    {
        if (GameManager.manager.enemiesAlive >= _numberOfEnemiesMax) return false;
        return true;
    }
}
