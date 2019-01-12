using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserShot : MonoBehaviour
{
    [Header("Gameplay Datas")]
    [SerializeField] int _hitValue = 1;
    private bool _isEnemy = true;
    bool _isActive = false;

    [Header("Personal Datas")]
    private Transform _transform;
    private Animator _animator;
    [SerializeField]
    private SpriteRenderer _spriteRdr;

    public void PreActiveMode()
    {
        if (_animator == null) _animator = this.gameObject.GetComponentInChildren<Animator>();
        _animator.SetTrigger("PrepTir");
    }

    public void ActiveMode(bool pIsEnemy)
    {
        _isEnemy = pIsEnemy;
        _isActive = true;
        
        if (_animator == null) _animator = this.gameObject.GetComponentInChildren<Animator>();
        _animator.SetBool("Attacking", true);
    }

    public void DesactiveMode()
    {
        _isActive = false;

        if (_animator == null) _animator = this.gameObject.GetComponentInChildren<Animator>();
        _animator.SetBool("Attacking", false);
    }
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isActive) return;

        Enemy enemyColl = collision.gameObject.GetComponent<Enemy>();

        if (enemyColl != null && !_isEnemy)
        {
            enemyColl.GetHit(_hitValue, enemyColl.transform.position);
        }

        Player playerColl = collision.gameObject.GetComponent<Player>();
        if (playerColl != null && _isEnemy)
        {
            playerColl.GetHit();
        }
    }
}
