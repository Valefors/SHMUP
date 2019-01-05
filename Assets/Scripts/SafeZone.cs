using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    static float MAX_X = 15f;
    static float MAX_Y = 15f;
    static float MIN_X = -15f;
    static float MIN_Y = -15f;

    public static bool IsOffField(Vector2 pPos)
    {
        if (pPos.x < MIN_X || pPos.x > MAX_X || pPos.y < MIN_Y || pPos.y > MAX_Y) return true;
        return false;
    }
}
