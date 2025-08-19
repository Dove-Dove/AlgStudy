using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Button StartButton;
    public Button TrunButton;
    public Button returnUiButton;
    public Button CallButton;

    public GameObject saveSlot;
    public GameObject saveScrollObj;
    public GameObject saveUi;
    bool saveUiActive = false;

    public List<RootSaveData> saveRoot = new List<RootSaveData>();

    // Start is called before the first frame update
    void Start()
    {
        StartButton.onClick.AddListener(StartEvent);
        TrunButton.onClick.AddListener(TrunEvent);
        returnUiButton.onClick.AddListener(SaveDataUi);
        CallButton.onClick.AddListener(CallEvent);
    }


    void StartEvent()
    {
        GameObject.Find("Unit").GetComponent<UnitController>().SetStartMoveEvnet();
    }
    void TrunEvent()
    {
        GameObject.Find("Unit").GetComponent<UnitController>().TrunMoveStackEvent();
    }

    void CallEvent()
    {
        GameObject.Find("Unit").GetComponent<UnitController>().CallMoveEvent();
    }

    void SaveDataUi()
    {
        saveUiActive = !saveUiActive;
        if(saveUiActive)
        {
            saveUi.SetActive(true);
            CreateSaveSlot();
        }
        else
        {
            saveUi.SetActive(false);
        }
    }

    void CreateSaveSlot()
    {
        for (int count = 0; count < saveRoot.Count; count++)
        {
            GameObject cSaveSlot = Instantiate(saveSlot, saveScrollObj.transform);
            cSaveSlot.GetComponentInChildren<SaveRootSlot>().slotSet(saveRoot[count], count);
        }
    }

    public void AddRootData(RootSaveData data)
    {
        saveRoot.Add(data);
    }
}
