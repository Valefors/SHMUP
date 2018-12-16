using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Personal Datas")]
    [SerializeField]
    protected Transform _spawnShot;
    protected Transform _transform;
    [SerializeField]
    protected GameObject _prefabShot;


    [Header("Gameplay Datas")]
    [SerializeField]
    protected float _speed;
    [SerializeField]
    protected float _shotRate = 0.1f;

    private bool _canShoot = true;

    private static string _VERTICAL_AXIS = "Vertical";
    private static string _HORIZONTAL_AXIS = "Horizontal";

    // Start is called before the first frame update
    protected void Start()
    {
        _transform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float lXmovValue = Input.GetAxis(_HORIZONTAL_AXIS);
        float lYmovValue = Input.GetAxis(_VERTICAL_AXIS);

        if (lXmovValue != 0 || lYmovValue != 0)
        {
            Move(lXmovValue, lYmovValue);
        }

        if (Input.GetAxisRaw("Fire1") != 0)
        {
            ManageShoot();
        }
    }

    void Move(float lXmovValue, float lYmovValue)
    {
        Vector3 lMovement = new Vector3(lXmovValue, lYmovValue, 0);
        lMovement = lMovement.normalized * _speed * Time.deltaTime;

        _transform.Translate(lMovement);
    }

    #region Shoot_region
    protected virtual void ManageShoot()
    {
        if (_canShoot)
        {
            Shoot();
        }
    }

    protected virtual void Shoot()
    {
        Shot shotShot = Instantiate(_prefabShot, _spawnShot.position, Quaternion.identity).GetComponent<Shot>();
        shotShot.SetUp(false, Vector3.up);
        _canShoot = false;
        Invoke("CanShootAgain", _shotRate);
    }

    protected virtual void CanShootAgain()
    {
        _canShoot = true;
    }

    #endregion

    #region GetDamage
    public virtual void GetHit()
    {
        Debug.Log("Player get hit !");
    }
    #endregion

}
