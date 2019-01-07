using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Player))]
public class PlayerEditor : Editor
{
    SerializedProperty horizAccCurve;
    SerializedProperty horizDecCurve;
    SerializedProperty horizAccSpeed;
    SerializedProperty horizDecSpeed;

    SerializedProperty vertAccCurve;
    SerializedProperty vertDecCurve;
    SerializedProperty vertAccSpeed;
    SerializedProperty vertDecSpeed;

    void OnEnable()
    {
        horizAccCurve = serializedObject.FindProperty("_horizontalAccelerationCurve");
        horizDecCurve = serializedObject.FindProperty("_horizontalDecelerationCurve");
        horizAccSpeed = serializedObject.FindProperty("_horizontalAccSpeed");
        horizDecSpeed = serializedObject.FindProperty("_horizontalDecSpeed");

        vertAccCurve = serializedObject.FindProperty("_verticalAccelerationCurve");
        vertDecCurve = serializedObject.FindProperty("_verticalDecelerationCurve");
        vertAccSpeed = serializedObject.FindProperty("_verticalAccSpeed");
        vertDecSpeed = serializedObject.FindProperty("_verticalDecSpeed");
    }

    public override void OnInspectorGUI()
    {
        Player player = (Player)target;
        // prefab override logic works on the entire property.
        if (DrawDefaultInspector())
        {
            // spawn.RotateLeft();
        }

        //EditorGUILayout.LabelField("Movement Part",GUIStyle.);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(" ", GUILayout.Width(150));
        EditorGUILayout.LabelField("Vertic", GUILayout.Width(40));
        EditorGUILayout.LabelField("Horizo", GUILayout.Width(40));
        EditorGUILayout.EndHorizontal();
        //ACCELERATION PART 
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Acceleration Curve", GUILayout.Width(150));
        vertAccCurve.animationCurveValue = EditorGUILayout.CurveField(vertAccCurve.animationCurveValue, GUILayout.Width(40));
        horizAccCurve.animationCurveValue = EditorGUILayout.CurveField(horizAccCurve.animationCurveValue, GUILayout.Width(40));
        //EditorGUILayout.PropertyField(horizAccCurve, GUIContent.none, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Acceleration Speed", GUILayout.Width(150));
        vertAccSpeed.floatValue = EditorGUILayout.FloatField(vertAccSpeed.floatValue, GUILayout.Width(40));
        horizAccSpeed.floatValue = EditorGUILayout.FloatField(horizAccSpeed.floatValue, GUILayout.Width(40));
        //EditorGUILayout.PropertyField(horizAccCurve, GUIContent.none, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();


        //DECELERATION
        
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Deceleration Curve", GUILayout.Width(150));
        vertDecCurve.animationCurveValue = EditorGUILayout.CurveField(vertDecCurve.animationCurveValue, GUILayout.Width(40));
        horizDecCurve.animationCurveValue = EditorGUILayout.CurveField(horizDecCurve.animationCurveValue, GUILayout.Width(40));
        //EditorGUILayout.PropertyField(horizAccCurve, GUIContent.none, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Deceleration Speed", GUILayout.Width(150));
        vertDecSpeed.floatValue = EditorGUILayout.FloatField(vertDecSpeed.floatValue, GUILayout.Width(40));
        horizDecSpeed.floatValue = EditorGUILayout.FloatField(horizDecSpeed.floatValue, GUILayout.Width(40));
        //EditorGUILayout.PropertyField(horizAccCurve, GUIContent.none, GUILayout.Width(50));
        EditorGUILayout.EndHorizontal();
        


        serializedObject.ApplyModifiedProperties();
    }
}