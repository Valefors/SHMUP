using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shaker : MonoBehaviour
{
    [Header("Player hit shake")]
    [Range(0f, 5f)]
    [SerializeField] float _intensityX;
    [Range(0f, 5f)]
    [SerializeField] float _intensityY;
    [SerializeField] float _duration = 1f;

    [Header("Ennemy killed")]
    [Range(0f, 5f)]
    [SerializeField] float _littleIntensityX;
    [Range(0f, 5f)]
    [SerializeField] float _littleIntensityY;
    [SerializeField] float _littleDuration = 1f;

    [Header("Final shake")]
    [SerializeField] private float _finalShakeDuration;
    [SerializeField] private AnimationCurve _finalShakeX;
    [SerializeField] private AnimationCurve _finalShakeY;


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
            Vector3 lRandomPoint = new Vector3(_initialPos.x + (Random.Range(-1f, 1f) * _intensityX), _initialPos.y + (Random.Range(-1f, 1f) * _intensityY), _initialPos.z);
            _target.localPosition = lRandomPoint;
            yield return null;
        }

        _target.localPosition = _initialPos;
        _isShaking = false;
    }

    public void LittleShake()
    {
        if (!_isShaking) StartCoroutine(DoLittleShake());
    }

    IEnumerator DoLittleShake()
    {
        _isShaking = true;
        float lStartTime = Time.realtimeSinceStartup;

        while (Time.realtimeSinceStartup < lStartTime + _littleDuration)
        {
            Vector3 lRandomPoint = new Vector3(_initialPos.x + (Random.Range(-1f, 1f) * _littleIntensityX), _initialPos.y + (Random.Range(-1f, 1f) * _littleIntensityY), _initialPos.z);
            _target.localPosition = lRandomPoint;
            yield return null;
        }

        _target.localPosition = _initialPos;
        _isShaking = false;
    }

    public void FinalShake()
    {
        StartCoroutine(DoFinalShake());
    }

    IEnumerator DoFinalShake()
    {
        _isShaking = true;
        float lStartTime = Time.realtimeSinceStartup;
        float finishTime = lStartTime + _finalShakeDuration;

        while (Time.realtimeSinceStartup < finishTime)
        {
            Vector2 lIntensity = Vector2.zero;
            float lerpCustomValue = (finishTime - Time.realtimeSinceStartup) / _finalShakeDuration;
            //Debug.Log("Lerp value : " + lerpCustomValue);
            lIntensity.x = _finalShakeX.Evaluate(lerpCustomValue);
            lIntensity.y = _finalShakeY.Evaluate(lerpCustomValue); 
             Vector3 lRandomPoint = new Vector3(_initialPos.x + (Random.Range(-1f, 1f) * lIntensity.x), _initialPos.y + (Random.Range(-1f, 1f) * lIntensity.y), _initialPos.z);
            _target.localPosition = lRandomPoint;
            yield return null;
        }

        _target.localPosition = _initialPos;
        _isShaking = false;
    }
}
