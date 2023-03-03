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
        {
            combo = 0;
            StartCoroutine(ComboStart());
        }
        
        if (comboStart) 
            combo++;
    }

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

        //yield return new WaitUntil(()=>GameManager.Inst.Get_CutGameStart);

    }
}

