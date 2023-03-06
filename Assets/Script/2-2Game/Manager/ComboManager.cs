using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    private int combo = 0;              // 현재 콤보
    private bool comboStart = false;    // 콤보가 시작됬는지 확인하는 bool값

    // 과일을 자를시 호출
    public void Combo(float _saveTime)
    {
        // 콤보가 시작되지 않았다면 시작시키고
        // 시작되어있다면 콤보수 증가
        if (!comboStart)
        {
            combo = 0;
            StartCoroutine(ComboStart());
        }
        if (comboStart) 
            combo++;
    }

    // 콤보가 종료되었을때 3콤보 이상이라면 콤보 텍스트 표시 및 점수 추가
    private WaitForSeconds wait1 = new WaitForSeconds(1f);
    IEnumerator ComboStart()
    {
        comboStart = true;
        yield return wait1;
        comboStart = false;

        if (combo >= 3 )
        {
            GameManager.Inst.Get_UiManager.ComboText(combo);
            GameManager.Inst.ComboScore(combo);
            combo = 0;
        }
    }
}

