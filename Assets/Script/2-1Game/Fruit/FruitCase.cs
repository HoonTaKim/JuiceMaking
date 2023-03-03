using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// ������ ���� �� ��ü.
/// </summary>
public class FruitCase : MonoBehaviour
{
    public int case_Id = 0;

    private SelectFruit_UiManager uiManager = null;
    [SerializeField] private TextMeshProUGUI numText = null;
    private int cupMaxCount = 0;
    private int curCupCount = 0;

    private bool caseOn = false;

    public int Get_CaseId { get { return case_Id; } private set { } }
    public bool Get_CaseOn { get { return caseOn; } private set { } }
    public int Get_CupCount { get { return curCupCount; } private set { } }
    public int Get_CupMaxCount { get { return cupMaxCount; } private set { } }

    //�������ϰ� public���� �س����״� ���Ѵٸ� ���߿� property�� �ٲٽÿ�
    public FRUITCOLOR color = FRUITCOLOR.DEFAULT;
    public bool isRight;

    public void SetCupMaxCount(int _count)
    {
        cupMaxCount = _count;
        curCupCount = cupMaxCount;
        uiManager.FruitCupText(numText, curCupCount);
    }

    private void Awake()
    {
        uiManager = GameManager.Inst.Get_SelectFruitUiManager;
    }

    public void CountChange(int _num)
    {
        if (_num > 1) curCupCount = cupMaxCount;
        else curCupCount += _num;
        uiManager.FruitCupText(numText, curCupCount);
    }

    public bool Set_CaseOn(bool _off)
    {
        caseOn = _off;
        return caseOn;
    }
}
