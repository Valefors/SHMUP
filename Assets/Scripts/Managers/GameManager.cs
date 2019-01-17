using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public class GameManager : MonoBehaviour
{
    [SerializeField] int _levelToLoad = 1;

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
        _isPlaying = true;
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
    }

    public void Quit()
    {
        Application.Quit();
    }

    void OnDisable()
    {
        EventManager.StopListening(EventManager.PAUSE_EVENT, Pause);
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
