using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    Node[] _pathNode;
    public List<Vector3> nodesPosition = new List<Vector3>();

    public List<Quaternion> nodesRotation = new List<Quaternion>(); // TEST POUR LE LD AXEL

    public int currentNode;

    static Vector3 _currentPositionHolder;

    private void Start()
    {
        _pathNode = GetComponentsInChildren<Node>();

        FileArray();
    }

    public void FileArray()
    {
        Quaternion savedRotation;
        for (int i = 0; i < _pathNode.Length; i++)
        {
            nodesPosition.Add(_pathNode[i].transform.position);
            savedRotation = _pathNode[i].transform.rotation;
            nodesRotation.Add(savedRotation); // TEST LD AXEL
            _pathNode[i].gameObject.SetActive(false);
        }
    }

    void DrawLine()
    {
        for (int i = 0; i < _pathNode.Length - 1; i++)
        {
            Debug.DrawLine(nodesPosition[i], nodesPosition[i + 1], Color.green);
        }

        Debug.DrawLine(nodesPosition[nodesPosition.Count - 1], nodesPosition[0], Color.green);
    }

    private void Update()
    {
        DrawLine();
    }
}
