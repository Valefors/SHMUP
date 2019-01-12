using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    static float MAX_X = 12f;
    static float MAX_Y = 8f;
    static float MIN_X = -12f;
    static float MIN_Y = -9f;

    /*
         static float MAX_X = 10f;
    static float MAX_Y = 8f;
    static float MIN_X = -10f;
    static float MIN_Y = -9f;
    */

    public static bool IsOffField(Vector2 pPos)
    {
        if (pPos.x < MIN_X || pPos.x > MAX_X || pPos.y < MIN_Y || pPos.y > MAX_Y) return true;
        return false;
    }
}
