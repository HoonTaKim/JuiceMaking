using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class OrderSheetInfo
{
    public string juiceName;
    public Texture juice_Texture;
}

public class OrderSheetData : MonoBehaviour
{
    [SerializeField] private List<OrderSheetInfo> ordersheetInfoList = new List<OrderSheetInfo>();
    [SerializeField] private List<RawImage> orderSheetList = new List<RawImage>();

    // Start is called before the first frame update
    void Start()
    {
        OrderSheetSeting();
    }

    public void OrderSheetSeting()
    {
        int idx = 0;
        for (int i = 0; i < orderSheetList.Count; i++)
        {
            idx = Random.Range(0, ordersheetInfoList.Count);
            orderSheetList[i].GetComponent<RawImage>().texture = ordersheetInfoList[idx].juice_Texture;
            orderSheetList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = ordersheetInfoList[idx].juiceName;

            ordersheetInfoList.Remove(ordersheetInfoList[idx]);
        }
    }
}
