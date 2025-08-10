using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Button StartButton;

    // Start is called before the first frame update
    void Start()
    {
        StartButton.onClick.AddListener(StartEvent);
    }


    void StartEvent()
    {
        GameObject.Find("Unit").GetComponent<UnitController>().SetStartMoveEvnet();
    }
}
