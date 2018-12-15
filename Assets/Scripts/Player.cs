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
            Vector3 lMovement = new Vector3(lXmovValue, lYmovValue,0);
            lMovement = lMovement.normalized * _speed * Time.deltaTime;
            
            _transform.Translate(lMovement);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Instantiate(_prefabShot, _spawnShot.position, Quaternion.identity);
        }

    }
}
