using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int _levelToLoad = 1;
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

    // Start is called before the first frame update
    void OnEnable()
    {
        if(_manager == null) _manager = this;
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
}
