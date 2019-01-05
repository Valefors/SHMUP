using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [Header("Gameplay datas")]
    [Range(0f, 2f)]
    [SerializeField] float _intensity;
    [SerializeField] float _duration = 1f;

    bool _isShaking = false;

    [Header("Personal datas")]
    Transform _target;
    Vector3 _initialPos;

    private static Shaker _instance;

    public static Shaker instance {
        get { return _instance; }
    }

    private void Awake()
    {
        if (_instance == null) _instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _target = GetComponent<Transform>();
        _initialPos = _target.localPosition;
    }

    public void Shake()
    {
        if(!_isShaking) StartCoroutine(DoShake());
    }

    IEnumerator DoShake()
    {
        _isShaking = true;
        float lStartTime = Time.realtimeSinceStartup;

        while(Time.realtimeSinceStartup < lStartTime + _duration)
        {
            Vector3 lRandomPoint = new Vector3(Random.Range(-1f, 1f) * _intensity, Random.Range(-1f, 1f) * _intensity, _initialPos.z);
            _target.localPosition = lRandomPoint;
            yield return null;
        }

        _target.localPosition = _initialPos;
        _isShaking = false;
    }
}
