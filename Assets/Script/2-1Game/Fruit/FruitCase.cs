using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// 과일이 담기는 컵 객체.
/// </summary>
public class FruitCase : MonoBehaviour
{
    public int case_Id = 0;

    private SelectFruit_UiManager uiManager = null;
    [SerializeField] private TextMeshProUGUI numText = null;    // 들어갈 수 있는 과일의 개수 표시
    private int cupMaxCount = 0;                                // 컵에 들어갈 수 있는 과일의 최대 개수
    private int curCupCount = 0;                                // 현재 컵에 들어간 과일의 개수

    private bool caseOn = false;
    public FRUITCOLOR color = FRUITCOLOR.DEFAULT;               // 해당 컵의 색 저장
    public bool isRight;                                        // 색이 들어갔는지 확인용 bool 값

    public int Get_CaseId { get { return case_Id; } private set { } }
    public bool Get_CaseOn { get { return caseOn; } private set { } }
    public int Get_CupCount { get { return curCupCount; } private set { } }
    public int Get_CupMaxCount { get { return cupMaxCount; } private set { } }

    private void Awake()
    {
        uiManager = GameManager.Inst.Get_SelectFruitUiManager;
    }

    // 현재 남은 과일의 개수를 표시
    public void SetCupMaxCount(int _count)
    {
        cupMaxCount = _count;
        curCupCount = cupMaxCount;
        uiManager.FruitCupText(numText, curCupCount);
    }

    // 값 초기화용
    public void CountChange(int _num)
    {
        if (_num > 1) curCupCount = cupMaxCount;
        else curCupCount += _num;
        uiManager.FruitCupText(numText, curCupCount);
    }

    // 갯수가 0이라면 과일이 더이상 못들어오게 막는 함수
    public bool Set_CaseOn(bool _off)
    {
        caseOn = _off;
        return caseOn;
    }
}
