using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Button StartButton;
    public Button TrunButton;

    // Start is called before the first frame update
    void Start()
    {
        StartButton.onClick.AddListener(StartEvent);
        TrunButton.onClick.AddListener(TrunEvent);
    }


    void StartEvent()
    {
        GameObject.Find("Unit").GetComponent<UnitController>().SetStartMoveEvnet();
    }
    void TrunEvent()
    {
        //GameObject.Find("Unit").GetComponent<UnitController>().TrunMoveEvnet();
        GameObject.Find("Unit").GetComponent<UnitController>().TrunMoveStackEvent();
    }
}
