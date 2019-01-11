using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Spawn))]
public class Test_SpawnEditor : Editor
{
    public override void OnInspectorGUI()
    {
        Spawn spawn = (Spawn)target;

        if (DrawDefaultInspector())
        {
           // spawn.RotateLeft();
        }

        if (GUILayout.Button("Rotate Left"))
        {
            spawn.RotateLeft();
        }

        if (GUILayout.Button("Rotate Right"))
        {
            spawn.RotateRight();
        }

        if (GUILayout.Button("Add Canon"))
        {
            spawn.AddModule1();
        }

        if (GUILayout.Button("Add V-Gun"))
        {
            spawn.AddModule2();
        }

        if (GUILayout.Button("Add Laser"))
        {
            spawn.AddModule3();
        }

        if (GUILayout.Button("Add Scrap"))
        {
            spawn.AddModule4();
        }

        if (GUILayout.Button("Remove Module"))
        {
            spawn.RemoveModule();
        }
    }
}
