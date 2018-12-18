using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _spriteRdr;

    [SerializeField]
    private float _speedShot = 1f;
    private bool _isEnemy = true;

    [SerializeField]
    private Vector3 _direction = Vector3.up;



    private Transform _transform;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Collision with " + collision.gameObject.name);

        Enemy enemyColl = collision.gameObject.GetComponent<Enemy>();
        if (enemyColl != null && !_isEnemy)
        {
            enemyColl.GetHit();
            this.Touch();
        }

        Player playerColl = collision.gameObject.GetComponent<Player>();
        if (playerColl != null && _isEnemy)
        {
            playerColl.GetHit();
            this.Touch();
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        _transform.Translate(_direction * _speedShot * Time.deltaTime);
    }

    //__TO DO : To change PROVISOIRE
    public void SetUp(bool isEnemy, Quaternion rotation)
    {
        SetUp(isEnemy, rotation, _speedShot);
    }

    public void SetUp(bool isEnemy, Quaternion rotation, float speed)
    {
        this._isEnemy = isEnemy;
        this.transform.rotation = rotation;
        this._speedShot = speed;

        this._direction = Vector3.up;

        ChangeColor();
    }

    public void ChangeColor()
    {
        if (_isEnemy)
        {
            _spriteRdr.color = Color.red;
        }
        else
        {
            _spriteRdr.color = Color.yellow;
        }
    }


    private void Touch()
    {
        Deactivate();
    }

    private void Deactivate()
    {
        Destroy(this.gameObject);
    }

}
