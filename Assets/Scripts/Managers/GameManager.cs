using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GameManager : MonoBehaviour
{
    [Header("PAUSE GAME FEEL")]
    [SerializeField] float _pauseValue = 0.12f;

    [SerializeField] int _levelToLoad = 1;

    [SerializeField] public Slider BossHealthBar;

    public bool isLD = false;

    [HideInInspector] public Transform scrolling;
    [HideInInspector] public Vector3 scrollingVector = new Vector3(0, -12, 0);

    //TEST
    [SerializeField] RectTransform _loadingScreen;
    [SerializeField] Slider _slider;

    [HideInInspector] public bool isPlaying {
        get {
            return _isPlaying;
        }
    }

    private bool _isPlaying = false;

    private static GameManager _manager;
    public static GameManager manager {
        get {
            return _manager;
        }
    }

    public int enemiesAlive=0;
    public bool hasFilled = false;
    bool startFill = false;
    int bossMaxHealth;
    public Image fill;

    private void Awake()
    {
        if (_manager == null) _manager = this;

        else if (_manager != this) Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(EventManager.GAME_OVER_EVENT, GameOver);
        EventManager.StartListening(EventManager.PAUSE_EVENT, Pause);
        EventManager.StartListening(EventManager.END_TUTORIAL_EVENT, EndTutorial);

        _isPlaying = true;
    }

    public void UpdateBossHealth(int newHealth)
    {
        BossHealthBar.value = newHealth;
        Color c = new Color(fill.color.r, fill.color.g, fill.color.b);
        c.g = (newHealth*0.005f);
        fill.color = c;
    }

    public void EnableBossHealthBar(int maxHealth)
    {
        BossHealthBar.gameObject.SetActive(true);
        BossHealthBar.value = 0;
        BossHealthBar.maxValue = maxHealth;
        bossMaxHealth = maxHealth;
        startFill = true;
    }

    public void DisableBossHealthBar()
    {
        BossHealthBar.gameObject.SetActive(false);
    }   

    public void Play()
    {
        StartCoroutine(LoadAsynchronously(_levelToLoad));
    }

    IEnumerator LoadAsynchronously(int pSceneIndex)
    {
        AsyncOperation lOperation = SceneManager.LoadSceneAsync(pSceneIndex);

        _loadingScreen.gameObject.SetActive(true);

        while (!lOperation.isDone)
        {
            float lProgress = Mathf.Clamp01(lOperation.progress / 0.9f);
            _slider.value = lProgress;
            
            yield return null;
        }

        _loadingScreen.gameObject.SetActive(false);

        if (pSceneIndex == _levelToLoad) LaunchGame();
    }

    public void LaunchGame()
    {
        EventManager.TriggerEvent(EventManager.PLAY_EVENT);
        Tutorial();
    }

    void Tutorial()
    {
        _isPlaying = false;
        EventManager.TriggerEvent(EventManager.TUTORIAL_EVENT);
    }

    void EndTutorial()
    {
        _isPlaying = true;
    }

    private void Pause()
    {
        _isPlaying = false;
    }

    public void Resume()
    {
        _isPlaying = true;
        EventManager.TriggerEvent(EventManager.RESUME_EVENT);
    }

    public void Menu()
    {
        StartCoroutine(LoadAsynchronously(_levelToLoad - 1));
        EventManager.TriggerEvent(EventManager.MENU_EVENT);
    }

    public void GameOver()
    {
        _isPlaying = false;
        enemiesAlive = 0;
        hasFilled = false;
    }

    public void BossDefeated()
    {
        DisableBossHealthBar();
        _isPlaying = false;
    }

    public void PauseFeel()
    {
       StartCoroutine(PauseCoroutine());
    }

    private IEnumerator PauseCoroutine()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(_pauseValue);
        Time.timeScale = 1;
        yield return null;
    }

    public void Quit()
    {
        Application.Quit();
    }

    void OnDisable()
    {
        EventManager.StopListening(EventManager.PAUSE_EVENT, Pause);
        EventManager.StopListening(EventManager.GAME_OVER_EVENT, GameOver);
        EventManager.StopListening(EventManager.END_TUTORIAL_EVENT, EndTutorial);
    }

    private void FixedUpdate()
    {
        if ((startFill && !hasFilled) && BossHealthBar.value < bossMaxHealth)
        {
            BossHealthBar.value += Time.deltaTime * 100;
        }
        else if ((startFill && !hasFilled) && BossHealthBar.value >= bossMaxHealth)
        {
            BossHealthBar.value = bossMaxHealth;
            hasFilled = true;
            startFill = false;
        }
    }

#if UNITY_EDITOR
    [ExecuteInEditMode]
    void Update()
    {
        if (!UnityEditor.EditorApplication.isPlaying)
        {
            Vector3[] bounds = SafeZone.Bounds(-2);

            Debug.DrawLine(bounds[0], bounds[1], Color.green);
            Debug.DrawLine(bounds[0], bounds[3], Color.green);
            Debug.DrawLine(bounds[2], bounds[1], Color.green);
            Debug.DrawLine(bounds[2], bounds[3], Color.green);
        }
    }
#endif
}
