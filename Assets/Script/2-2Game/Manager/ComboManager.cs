using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComboManager : MonoBehaviour
{
    private int combo = 0;              // ���� �޺�
    private bool comboStart = false;    // �޺��� ���ۉ���� Ȯ���ϴ� bool��

    // ������ �ڸ��� ȣ��
    public void Combo(float _saveTime)
    {
        // �޺��� ���۵��� �ʾҴٸ� ���۽�Ű��
        // ���۵Ǿ��ִٸ� �޺��� ����
        if (!comboStart)
        {
            combo = 0;
            StartCoroutine(ComboStart());
        }
        if (comboStart) 
            combo++;
    }

    // �޺��� ����Ǿ����� 3�޺� �̻��̶�� �޺� �ؽ�Ʈ ǥ�� �� ���� �߰�
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

