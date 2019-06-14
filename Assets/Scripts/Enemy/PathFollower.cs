using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollower : MonoBehaviour
{
    Node[] _pathNode;
    public List<Vector3> nodesPosition = new List<Vector3>();
    public List<Quaternion> nodesRotation = new List<Quaternion>();
    public List<int> nodesWaitTime = new List<int>();

    public int currentNode;

    public bool draw = true;

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

            Vector3 test = _pathNode[i].transform.localEulerAngles;
            test.z += _pathNode[i].transform.parent.parent.transform.localEulerAngles.z;
            savedRotation = Quaternion.Euler(test);
          
            nodesRotation.Add(savedRotation);

            nodesWaitTime.Add(_pathNode[i].waitTime);
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
        if(draw)
            DrawLine();
    }
}
