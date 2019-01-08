using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] int _levelToLoad = 1;

    private static GameManager _manager;
    public static GameManager manager {
        get {
            return _manager;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void Play()
    {
        EventManager.TriggerEvent(EventManager.PLAY_EVENT);
        SceneManager.LoadScene(_levelToLoad);
    }

    /*private void Pause()
    {
        EventManager.TriggerEvent(EventManager.PAUSE_EVENT);
    }*/

    public void Resume()
    {
        print("here");
        EventManager.TriggerEvent(EventManager.RESUME_EVENT);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
