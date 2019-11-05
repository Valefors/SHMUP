using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShot : MonoBehaviour
{
    [Header("Gameplay Datas")]
    [SerializeField] int _hitValue = 1;
    private bool _isEnemy = true;
    [HideInInspector]
    public bool isActive = false;

    [Header("Personal Datas")]
    private Transform _transform;
    private Animator _animator;
    [SerializeField]
    private SpriteRenderer _spriteRdr;

    private bool isWaiting = false;
    private float count=0;

    public void ResetAnim()
    {
        if (_animator == null) _animator = this.gameObject.GetComponentInChildren<Animator>();
        _animator.SetTrigger("Reset");
    }
    
    public void PreActiveMode()
    {
        if (_animator == null) _animator = this.gameObject.GetComponentInChildren<Animator>();
        _animator.SetTrigger("PrepTir");
    }

    public void ActiveMode(bool pIsEnemy)
    {
        _isEnemy = pIsEnemy;
        isActive = true;
        
        if (_animator == null) _animator = this.gameObject.GetComponentInChildren<Animator>();
        _animator.SetBool("Attacking", true);
    }

    public void DesactiveMode()
    {
        isActive = false;

        if (_animator == null) _animator = this.gameObject.GetComponentInChildren<Animator>();
        _animator.SetBool("Attacking", false);
    }


    public bool GetSide()
    {
        return _isEnemy;
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!isActive) return;

        Enemy enemyColl = collision.gameObject.GetComponent<Enemy>();

        if (enemyColl != null && !_isEnemy && !isWaiting)
        {
            enemyColl.GetHit(_hitValue, enemyColl.transform.position);
            isWaiting = true;
        }
        
    }

    private void Update()
    {
        if(isWaiting)
        {
            count += Time.deltaTime;
            if (count >= 0.3f)
            {
                isWaiting = false;
                count = 0;
            }
        }
    }
}
