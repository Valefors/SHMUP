using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField]
    private float _speedShot = 1f;
    private bool _isEnemy = true;

    [SerializeField]
    private Vector3 _direction = Vector3.up;

    int _hitValue = 1;

    private Transform _transform;
    [SerializeField]
    private Animator _animator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameManager.manager.isPlaying) return;

        Enemy enemyColl = collision.gameObject.GetComponent<Enemy>();
        if (enemyColl != null && !_isEnemy)
        {
            AkSoundEngine.PostEvent("Reached", gameObject);
            enemyColl.GetHit(_hitValue, this.transform.position);
            this.Touch();
        }

        
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
        EventManager.StartListening(EventManager.GAME_OVER_EVENT, GameOver);
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.manager.isPlaying) return;

        if (SafeZone.IsOffField(_transform.position))
        {
            //Destroy(gameObject);
            return;
        }

        //print("_direction " + _direction + "_speedShot " + _speedShot + " Time.deltaTime " + Time.deltaTime + " scale : "+ Time.timeScale);
        _transform.Translate(_direction * _speedShot * Time.deltaTime);
    }

    public bool GetSide()
    {
        return _isEnemy;
    }

    public void SetUp(bool pIsEnemy, Quaternion pRotation, float pSpeed, int pHitValue)
    {
        if(_transform == null) _transform = GetComponent<Transform>(); //Weird error

        _isEnemy = pIsEnemy;
        _transform.rotation = pRotation;
        _speedShot = pSpeed;
        _hitValue = pHitValue;

        _direction = Vector3.up;

        ChangeColor();
    }

    public void ChangeColor()
    {
       _animator.SetBool("enn", _isEnemy);
    }

    public void Touch()
    {
        Deactivate();
    }

    private void Deactivate()
    {
        Destroy(this.gameObject);
    }

    void GameOver()
    {
        Destroy(gameObject);
    }

    private void OnDisable()
    {
        EventManager.StopListening(EventManager.GAME_OVER_EVENT, GameOver);
    }

}
