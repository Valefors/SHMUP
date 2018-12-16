using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Personal Datas")]
    [SerializeField]
    private Transform _spawnShot;
    private Transform _transform;
    [SerializeField]
    private GameObject _prefabShot;


    [Header("Gameplay Datas")]
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _shotRate = 0.1f;



    private bool _canShoot = true;

    private static string _VERTICAL_AXIS = "Vertical";
    private static string _HORIZONTAL_AXIS = "Horizontal";
    // Start is called before the first frame update
    void Start()
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

        if (Input.GetMouseButton(0))
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

    void ManageShoot()
    {
        if (_canShoot)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Shot shotShot = Instantiate(_prefabShot, _spawnShot.position, Quaternion.identity).GetComponent<Shot>();
        shotShot.SetUp(false, Vector3.up);
        _canShoot = false;
        Invoke("CanShootAgain", _shotRate);
    }

    void CanShootAgain()
    {
        _canShoot = true;
    }



    #region GetDamage
    public void GetHit()
    {
        Debug.Log("I'm getting hit ! Outch !");
        //pv--;
        //Death();
    }

    #endregion
}
