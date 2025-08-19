using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SaveRootSlot : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI RootName;
    public Button CallButton;

    public TextMeshProUGUI StartRoot;
    public TextMeshProUGUI EndRoot;

    private RootSaveData slotSave;
    void Start()
    {
        CallButton.onClick.AddListener(Root);
    }
    public void slotSet(RootSaveData setData, int n)
    {
        slotSave = setData;

        string startText = setData.SaveRoot[0].x.ToString("0.00") + setData.SaveRoot[0].z.ToString("0.00");
        string endText = setData.SaveRoot[setData.SaveRoot.Count - 1].x.ToString("0.00") +
            setData.SaveRoot[setData.SaveRoot.Count - 1].z.ToString("0.00");

        RootName.text = $"Number {n +1 }";

        StartRoot.text = startText;
        EndRoot.text = endText;

    }

    public void Root()
    {
        GameObject.Find("SaveRoot").GetComponent<RootSave>().RootCall(slotSave);
        Debug.Log("dsafasdf");
    }
}
