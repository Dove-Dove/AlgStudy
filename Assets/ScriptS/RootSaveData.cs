using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Scriptable/root Data", order = int.MaxValue)]
public class RootSaveData : ScriptableObject
{
    [SerializeField]
    private string rootName;
    public string RootName => rootName;

    [SerializeField]
    private List<Vector3> saveRoot;
    public List<Vector3> SaveRoot => saveRoot;

}
