using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    private int combo = 0;
    private float saveTime = 0f;
    private float comboTime = 0f;
    private bool comboStart = false;

    public void Combo(float _saveTime)
    {
        saveTime = _saveTime;


        if (!comboStart) 
            StartCoroutine(ComboStart());
        
        if (comboStart) 
            combo++;
    }

    IEnumerator ComboStart()
    {
        Debug.Log("코루틴 시작?");
        comboStart = true;
        yield return new WaitForSeconds(0.35f);
        comboStart = false;

        if (combo >= 3 )
        {
            GameManager.Inst.Get_UiManager.ComboText(combo);
            GameManager.Inst.ComboScore(combo);
            combo = 0;
        }

        //yield return new WaitUntil(()=>GameManager.Inst.Get_CutGameStart);

    }
}

