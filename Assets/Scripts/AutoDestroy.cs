using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [SerializeField]
    private float timer = 0;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitBeforeDestroy());
    }
    
    IEnumerator WaitBeforeDestroy()
    {
        if(timer != 0)
        {
            yield return new WaitForSeconds(timer);
        }
        else if(this.GetComponent<ParticleSystem>() != null)
        {
            ParticleSystem lPS = this.GetComponent<ParticleSystem>();
            yield return new WaitWhile(() => lPS.isPlaying);
        }
        else
        {
            Debug.LogError("Autodestroy " + this.name + " immediately", this.gameObject);
        }
        Destroy(this.gameObject);
    }

}
