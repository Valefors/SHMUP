using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[ExecuteInEditMode]
public class GameManager : MonoBehaviour
{
    [SerializeField] int _levelToLoad = 1;

    public bool isLD = false;

    public Transform scrolling;
    [HideInInspector]
    public Vector3 scrollingVector = new Vector3(0, -12, 0);

    [HideInInspector] public bool isPause {
        get {
            return _isPause;
        }
    }

    private bool _isPause = false;

    private static GameManager _manager;
    public static GameManager manager {
        get {
            return _manager;
        }
    }

    private void Awake()
    {
        if (_manager == null) _manager = this;

        else if (_manager != this) Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        EventManager.StartListening(EventManager.PAUSE_EVENT, Pause);
    }

    public void Play()
    {
        EventManager.TriggerEvent(EventManager.PLAY_EVENT);
        SceneManager.LoadScene(_levelToLoad);
    }

    private void Pause()
    {
        _isPause = true;
    }

    public void Resume()
    {
        _isPause = false;
        EventManager.TriggerEvent(EventManager.RESUME_EVENT);
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
