using UnityEngine;
using System.Collections;
using System;

public static class StaticFunctions
{
    public static IEnumerator FadeIn(Action<float> myVariableResult, float fadeTime, Action callback = null)
    {
        float progress = 0f;
        float newValue = 0f;

        while (newValue < 1f)
        {
            newValue = Mathf.Lerp(0f, 1f, progress);
            myVariableResult(newValue);
            progress += Time.deltaTime / fadeTime;
            yield return null;
        }

        // Allow to call a method at the end of the coroutine
        if (callback != null)
        {
            callback();
        }
    }

    public static IEnumerator FadeOut(Action<float> myVariableResult, float fadeTime, Action callback = null)
    {
        float progress = 0f;
        float newValue = 1f;

        while (newValue > 0f)
        {
            newValue = Mathf.Lerp(1f, 0f, progress);
            myVariableResult(newValue);
            progress += Time.deltaTime / fadeTime;
            yield return null;
        }

        // Allow to call a method at the end of the coroutine
        if (callback != null)
        {
            callback();
        }
    }
}
