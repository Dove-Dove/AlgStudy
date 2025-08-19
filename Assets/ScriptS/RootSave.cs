using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class RootSave : MonoBehaviour
{
    public GameObject unit;
    private List<Vector3> currentPath = new List<Vector3>();
    private List<Vector3> callRoot = new List<Vector3>();


    private string rootName;

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
        string startText = currentPath[0].x.ToString() + currentPath[0].z.ToString();
        rootName = $"Start {startText} ";
        RootSaveData data = CreateRootSaveData();
        ItemSaveTool.SaveItemAsset(data, rootName);
        GameObject.Find("Canvas").GetComponent<UIManager>().AddRootData(data);

        var copy = new Stack<Vector3>(currentPath);
      
    }

    public RootSaveData CreateRootSaveData()
    {
        RootSaveData data = ScriptableObject.CreateInstance<RootSaveData>();

        void SetField(string name, object value)
        {
            typeof(RootSaveData).GetField(name, BindingFlags.NonPublic | BindingFlags.Instance)
                ?.SetValue(data, value);
        }

        SetField("rootName", rootName);
        SetField("saveRoot", new List<Vector3>(currentPath));

        return data;
    }

    public void RootClear()
    {
        currentPath.Clear();
    }

    public List<Vector3> TurnRootUnit()
    {
        return currentPath;
    }

    public void RootCall(RootSaveData call)
    {
        callRoot = call.SaveRoot;
    }

    public List<Vector3> SetCallRoot()
    {
        return callRoot;
    }

}

#if UNITY_EDITOR
//using UnityEditor;
#endif

public static class ItemSaveTool
{
#if UNITY_EDITOR
    public static void SaveItemAsset(RootSaveData rootSave, string fileName)
    {
        string dirPath = "Assets/RootSaveData";
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        string path = $"{dirPath}/{fileName}.asset";
        AssetDatabase.CreateAsset(rootSave, path);

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif
}


