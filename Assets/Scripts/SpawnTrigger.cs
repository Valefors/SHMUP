using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTrigger : MonoBehaviour
{
    [SerializeField] GameObject[] _wavesArray;
    [SerializeField] bool _isSkippable;
    [SerializeField] int _numberOfEnemiesMax;
    bool _isActive = false;

    [Tooltip("-1 if it's didn't change music part, the index of the part else")]
    [SerializeField] int _musicPartChange = -1;

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
            if (_musicPartChange != -1)
                AkSoundEngine.PostEvent("Music<" + _musicPartChange.ToString() + ">", gameObject);
            _isActive = true;
        }
    }

    void LaunchWaves()
    {
        for (int i = 0; i < _wavesArray.Length; i++)
        {
            if (!_isSkippable || CanLaunch(_wavesArray[i])) Instantiate(_wavesArray[i]);
        }
    }

    bool CanLaunch(GameObject wave)
    {
        int enemiesInWave = wave.GetComponentsInChildren<Enemy>().Length;
        if (GameManager.manager.enemiesAlive+enemiesInWave > _numberOfEnemiesMax) return false;
        return true;
    }
}
