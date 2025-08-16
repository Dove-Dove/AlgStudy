using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RootSave : MonoBehaviour
{
    public GameObject unit;
    private List<Vector3> currentPath = new List<Vector3>();

    private Stack<Stack<Vector3>> stackPath = new Stack<Stack<Vector3>>();


    public void saveingRoot(Vector3 root)
    {
        currentPath.Add(root);
    }

    public void endRoot(Vector3 root)
    {
        currentPath.Add(root);
        currentRootSave();
    }
     void currentRootSave()
    {
        var copy = new Stack<Vector3>(currentPath);
        stackPath.Push(copy);

        // 원본 초기화
        currentPath.Clear();
    }


    public Stack<Vector3> GetPath()
    {
        if (stackPath.Count != 0)
        {
            return stackPath.Peek();
        }

        return null;
    }
}
