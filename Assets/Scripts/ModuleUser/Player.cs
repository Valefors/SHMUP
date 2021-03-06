using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("Personal Datas")]
    protected Transform _transform;
    [SerializeField]
    protected Animator _animator;
    [SerializeField]
    protected Animator _flameAnimator;

    private static string _VERTICAL_AXIS = "Vertical";
    private static string _HORIZONTAL_AXIS = "Horizontal";

    [Header("Gameplay Datas")]
    [SerializeField]
    protected float _shotRate = 0.1f;

    [SerializeField]
    private List<Module> _modulesList = new List<Module>();
    private int _listLenght = 0;
    private bool _isInvicible = false;

    [Header("Movement Part")]
    [SerializeField]
    private float _speed;
    [SerializeField]
    private AnimationCurve _speedWeightCurve;
    private float _weight = 0f;

    [Header("Invulnerability")]
    [SerializeField] SpriteRenderer _invulnerabilitySprite;
    [SerializeField] float _invulnerabilityDelay;
    [SerializeField] int _numberModuleDecreaserInvulnerability;
    [SerializeField] int _invunerabilityPercentageDecrease;

    [HideInInspector] [SerializeField] private AnimationCurve _horizontalAccelerationCurve;
    [HideInInspector] [SerializeField] private AnimationCurve _horizontalDecelerationCurve;
    [HideInInspector] [SerializeField] private float _horizontalAccSpeed = 1;
    [HideInInspector] [SerializeField] private float _horizontalDecSpeed = 1;
    [Range(-1, 1)]
    private float _horizontalAccDecLerpValue;
    private Vector3 _horizontalLastMovement = Vector3.zero;

    [HideInInspector] [SerializeField] private AnimationCurve _verticalAccelerationCurve;
    [HideInInspector] [SerializeField] private AnimationCurve _verticalDecelerationCurve;
    [HideInInspector] [SerializeField] private float _verticalAccSpeed = 1;
    [HideInInspector] [SerializeField] private float _verticalDecSpeed = 1;
    [Range(-1, 1)]
    private float _verticalAccDecLerpValue;
    private Vector3 _verticalLastMovement = Vector3.zero;
    
    [SerializeField]
    private AnimationCurve _swingCurve;
    [SerializeField]
    private float _swingDegree;

    private float saveInvulnerableDelay;
    private float percentage;

    // Start is called before the first frame update
    private void Awake()
    {
        Camera.main.GetComponentInChildren<ParticleSystem>().Simulate(1);
        Camera.main.GetComponentInChildren<ParticleSystem>().Play();
    }

    private void Start()
    {
        Image[] tempList = null;
        GameManager.manager.BossHealthBar = GameObject.FindGameObjectWithTag("Finish").GetComponent<Slider>();
        if (GameManager.manager.BossHealthBar != null) tempList=GameManager.manager.BossHealthBar.GetComponentsInChildren<Image>();
        if(tempList!= null && tempList.Length>=2) GameManager.manager.fill = tempList[1];


        GameManager.manager.BossHealthBar.gameObject.SetActive(false);
        AkSoundEngine.PostEvent("Music", gameObject);
        

        saveInvulnerableDelay = _invulnerabilityDelay;
        percentage = (saveInvulnerableDelay * _invunerabilityPercentageDecrease) / 100;
        _transform = this.transform;
        _listLenght = _modulesList.Count;

        UpdateWeight();
    }

    // Update is called once per frame
    void Update()
    {
        if (_listLenght-1 >= _numberModuleDecreaserInvulnerability) _invulnerabilityDelay = saveInvulnerableDelay - (percentage * _listLenght) + percentage;
        else _invulnerabilityDelay = saveInvulnerableDelay;

        if (Input.GetKeyDown(KeyCode.G))
        {
            if (_isInvicible)
                GetInvicibility();
            else
                GetInvicibility(true);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(Photo(0.1f));
        }


        if (!GameManager.manager.isPlaying) return;

        SetModuleVoidMode();

        Move();

        if (Input.GetAxisRaw("Fire1") != 0)
        {
            SetModuleActionMode();
        }
    }

    #region Movement

    void Move()
    {
        Vector3 lMovement = Vector3.zero;
    
        float lXmovValue = Input.GetAxisRaw(_HORIZONTAL_AXIS);
        if (lXmovValue != 0)
            lMovement += HorizontalMove(lXmovValue);
        else if (_horizontalAccDecLerpValue != 0)
            lMovement += HorizontalSlowDown();

        float lYmovValue = Input.GetAxisRaw(_VERTICAL_AXIS);
        if (lYmovValue != 0)
            lMovement += VerticalMove(lYmovValue);
        else if (_verticalAccDecLerpValue != 0)
            lMovement += VerticalSlowDown();

        Vector2 lPoint = new Vector2(transform.position.x + lMovement.x, transform.position.y + lMovement.y);
        
        _flameAnimator.SetFloat("Vertical", _verticalAccDecLerpValue);
        _flameAnimator.SetFloat("Horizontal", _horizontalAccDecLerpValue);

        //if (SafeZone.IsOffField(lPoint)) return;

        if (SafeZone.IsOffFieldX(lPoint.x)) lMovement.x = 0;
        if (SafeZone.IsOffFieldY(lPoint.y)) lMovement.y = 0;
        if (lMovement != Vector3.zero)
        {
            AkSoundEngine.SetState("Moving_state", "yes");
            _transform.Translate(lMovement, Space.World);
        }
    }

    Vector3 HorizontalMove(float lXmovValue)
    {
        _horizontalAccDecLerpValue += Time.deltaTime * _horizontalAccSpeed * Mathf.Sign(lXmovValue);
        _horizontalAccDecLerpValue = Mathf.Clamp(_horizontalAccDecLerpValue, -1, 1);

        Vector3 lMovement = new Vector3(Mathf.Abs(lXmovValue), 0, 0);
        float lSpeed = _speedWeightCurve.Evaluate(_weight) * _speed;
        lMovement = lMovement.normalized * lSpeed * Time.deltaTime * Mathf.Sign(_horizontalAccDecLerpValue);
        
        _horizontalLastMovement = lMovement;

        lMovement *= _horizontalAccelerationCurve.Evaluate(Mathf.Abs(_horizontalAccDecLerpValue));
        //print(lMovement.x + "This ois here: " + _horizontalLastMovement.x + " and lerp = " + _horizontalAccelerationCurve.Evaluate(Mathf.Abs(_horizontalAccDecLerpValue)) + " lerp = " + _horizontalAccDecLerpValue + " (calculus = " + lMovement.normalized * lSpeed + ", " + Time.deltaTime + " , " + Mathf.Sign(_horizontalAccDecLerpValue));

        ChangeRotation();
        return lMovement;
    }

    Vector3 VerticalMove(float lYmovValue)
    {
        _verticalAccDecLerpValue += Time.deltaTime * _verticalAccSpeed * Mathf.Sign(lYmovValue);
        _verticalAccDecLerpValue = Mathf.Clamp(_verticalAccDecLerpValue, -1, 1);

        Vector3 lMovement = new Vector3(0, Mathf.Abs(lYmovValue), 0);
        float lSpeed = _speedWeightCurve.Evaluate(_weight) * _speed;
        lMovement = lMovement.normalized * lSpeed * Time.deltaTime * Mathf.Sign(_verticalAccDecLerpValue);

        _verticalLastMovement = lMovement;

        lMovement *= _verticalAccelerationCurve.Evaluate(Mathf.Abs(_verticalAccDecLerpValue));

        return lMovement;
    }

    Vector3 HorizontalSlowDown()
    {
        float pastLerp = _horizontalAccDecLerpValue;
        _horizontalAccDecLerpValue -= Time.deltaTime * _horizontalDecSpeed * Mathf.Sign(_horizontalAccDecLerpValue);
        if(Mathf.Sign(pastLerp) != Mathf.Sign(_horizontalAccDecLerpValue))
        {
            AkSoundEngine.SetState("Moving_state", "no");
            _horizontalAccDecLerpValue = 0;
        }

        Vector3 lMovement = _horizontalLastMovement;
        lMovement *= _horizontalDecelerationCurve.Evaluate(Mathf.Abs(_horizontalAccDecLerpValue));
        //print("SLOW = " + lMovement.x + " lastMovement = " + _horizontalLastMovement.x + " and lerp = " + _horizontalDecelerationCurve.Evaluate(Mathf.Abs(_horizontalAccDecLerpValue)) + " lerp = " + _horizontalAccDecLerpValue);

        ChangeRotation();
        return lMovement;
    }

    Vector3 VerticalSlowDown()
    {
        float pastLerp = _verticalAccDecLerpValue;
        _verticalAccDecLerpValue -= Time.deltaTime * _verticalDecSpeed * Mathf.Sign(_verticalAccDecLerpValue);
        if (Mathf.Sign(pastLerp) != Mathf.Sign(_verticalAccDecLerpValue))
        {
            AkSoundEngine.SetState("Moving_state", "no");
            _verticalAccDecLerpValue = 0;
        }

        Vector3 lMovement = _verticalLastMovement;
        lMovement *= _verticalDecelerationCurve.Evaluate(Mathf.Abs(_verticalAccDecLerpValue));

        return lMovement;
    }
    
    void ChangeRotation()
    {
        float lSwingValue =_horizontalAccDecLerpValue;

        Vector3 lEuler = _transform.rotation.eulerAngles;
        lEuler.z = _swingCurve.Evaluate(lSwingValue);
        _transform.rotation = Quaternion.Euler(lEuler);
    }

    #endregion


    void SetModuleVoidMode()
    {
        for (int i = 0; i < _listLenght; i++)
        {
            _modulesList[i].SetModeVoid();
        }
    }

    void SetModuleActionMode()
    {
        for (int i = 0; i < _listLenght; i++)
        {
            _modulesList[i].SetModeNormal();
        }
    }

    #region GetDamage
    public virtual void GetHit()
    {
        if (_isInvicible) return;

        Shaker.instance.Shake();
         
        if (_listLenght != 1)
        {
            RemoveLastModule();
            GetInvicibility();
        }
        else
        {
            if(!GameManager.manager.isLD) EventManager.TriggerEvent(EventManager.GAME_OVER_EVENT);
            AkSoundEngine.PostEvent("Death", gameObject);
            Debug.Log("This is a gameOver");
        }
    }

    void GetInvicibility(bool pIsGod = false)
    {
        _isInvicible = true;
        _animator.SetBool("Invulnerability", !pIsGod); // !pIsGod --> to avoid a flickering player when in god mode
        foreach (Module mod in _modulesList)
            mod.ModuleClignote(!pIsGod);
        _invulnerabilitySprite.gameObject.SetActive(true);
        if(!pIsGod) Invoke("GetNormal", _invulnerabilityDelay);
    }

    void GetNormal()
    {
        _isInvicible = false;
        _animator.SetBool("Invulnerability", false);
        foreach (Module mod in _modulesList)
            mod.ModuleClignote(false);
        _invulnerabilitySprite.gameObject.SetActive(false);
    }

    #endregion

    private void OnTriggerEnter2D(Collider2D pCol)
    {
        Shot shotCollided = pCol.gameObject.GetComponent<Shot>();
        if (shotCollided != null )
        {
            if (shotCollided.GetSide())
            {
                shotCollided.Touch();
                this.GetHit();
            }
        }
        LaserShot laserShotCollided = pCol.gameObject.GetComponent<LaserShot>();
        if (laserShotCollided != null)
        {
            if (laserShotCollided.GetSide() && laserShotCollided.isActive)
            {
                this.GetHit();
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D pCol)
    {
        if (!GameManager.manager.isPlaying) return;

        Module moduleCollided = pCol.gameObject.GetComponent<Module>();
        if (moduleCollided != null)
        {
            //need to know if there parent are still enemy (or even friend)
            if (moduleCollided.free)
            {
                AkSoundEngine.PostEvent("Get_module", gameObject);
                AddModule(moduleCollided);
            }
        }

        Enemy enemyColl = pCol.gameObject.GetComponent<Enemy>();
        if (enemyColl != null)
        {
            this.GetHit();
        }
    }

    private void AddModule(Module module)
    {
        if (module.GetComponent<ShooterModule>() != null) module.GetComponent<ShooterModule>().isEnemy = false;

        if(module.GetComponent<VGun>() != null)
        {
            AkSoundEngine.SetState("vGun_state", "yes");
        }

        module.transform.parent = _transform;

        if (module.rotateWhenPickUp)
        {
            Vector3 directionToLookAt = module.transform.position - _transform.position;
            //TO DO : need to make a clear feedback
            //something to make the module really go from start direction to this one
            module.transform.rotation = Quaternion.LookRotation(Vector3.forward, directionToLookAt);
        }
        module.free = false;

        _modulesList.Add(module);

        _listLenght++;

        UpdateWeight();
    }

    private void RemoveLastModule()
    {
        Module lModuleToDestroy = _modulesList[_listLenght - 1];
        _modulesList.RemoveAt(_listLenght - 1);
        _listLenght--;

        if(lModuleToDestroy.GetComponent<VGun>() != null)
        {
            AkSoundEngine.SetState("vGun_state", "no");
        }
        AkSoundEngine.PostEvent("Damaged", gameObject);

        lModuleToDestroy.SetDeathMode();

        UpdateWeight();
    }

    private void UpdateWeight()
    {
        float lNewWeight = 0;
        foreach (Module lModule in _modulesList)
        {
            lNewWeight += lModule._weight;
        }
        _weight = lNewWeight;

        AkSoundEngine.SetRTPCValue("Number_modules", _listLenght, gameObject);
    }

    public void DefeatedBoss()
    {
        GameManager.manager.BossDefeated();
        StartCoroutine(GoToPoint(new Vector3(0,-8,0), 3,0));
        StartCoroutine(Photo(7.9f));
        StartCoroutine(GoToPoint(new Vector3(0, 16, 0), 1, 9));
        //Go To A Point, stop shooting. Screenshot after a while
    }

    IEnumerator GoToPoint(Vector3 pointToGo, float timeToDoIt, float delay)
    {
        yield return new WaitForSeconds(delay);
        float lLerp = 0;
        Vector3 startPos = _transform.position;

        _flameAnimator.SetFloat("Vertical", 0);
        _flameAnimator.SetFloat("Horizontal", 0);

        _horizontalAccDecLerpValue = 0;
        _verticalAccDecLerpValue = 0;
        ChangeRotation();


        while (lLerp < 1)
        {
            lLerp += Time.deltaTime / timeToDoIt;
            _transform.position = Vector3.Lerp(startPos, pointToGo, lLerp);
            yield return new WaitForSeconds(0.01f);
        }
        _transform.position = pointToGo;
    }

    IEnumerator Photo(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        yield return new WaitForEndOfFrame();
        TakePhoto();
    }
    
    public void TakePhoto()
    {
        int lStartX = 1 * Screen.width / 4;
        int lStartY = 0;
        int lWidth = 1 * Screen.width / 2;
        int lHeight = 5 * Screen.height / 8;
        Texture2D lTex = new Texture2D(lWidth, lHeight, TextureFormat.RGB24, false);

        Rect lRect = new Rect(lStartX, lStartY, lWidth, lHeight);

        print(lRect + " lRect | " + lTex.height + " he --- wid " + lTex.width);
        lTex.ReadPixels(lRect, 0, 0, false);
        lTex.Apply();

        var bytes = lTex.EncodeToPNG();
        DestroyImmediate(lTex);


        string lPath = Application.dataPath + "FinalJunkScreenshot.png";
        System.IO.File.WriteAllBytes(lPath, bytes);
        Debug.Log("File saved at : " + lPath);
    }

}
