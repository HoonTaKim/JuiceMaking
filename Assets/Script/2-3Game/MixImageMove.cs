using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixImageMove : MonoBehaviour
{
    private MixImage2Move moveObj = null;
    private Vector3 saveTr = Vector3.zero;
    private UiManager uiManager = null;
    private Rigidbody2D rb = null;
    private bool pushOn = false;
    private bool createOn = false;

    private void Awake()
    {
        uiManager = FindObjectOfType<UiManager>();
        moveObj = GameManager.Inst.Get_MixerStart.movePos.GetComponent<MixImage2Move>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(GameManager.Inst.saveJuiceColor.r, GameManager.Inst.saveJuiceColor.g, GameManager.Inst.saveJuiceColor.b);
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(GameManager.Inst.saveJuiceColor.r, GameManager.Inst.saveJuiceColor.g, GameManager.Inst.saveJuiceColor.b);
        transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(GameManager.Inst.saveJuiceColor.r, GameManager.Inst.saveJuiceColor.g, GameManager.Inst.saveJuiceColor.b);
        createOn = false;
    }

    // �ϵ��ڵ� �����ؾߴ�
    private void Update()
    {
        // ���� ���� �߰��ϴ� ���� ����

        if (transform.position.x >= 12f && !createOn)
        {
            MixPooling.Inst.Out(transform.position.y);
            createOn = true;
        }

        if (transform.position.x > 15f) MixPooling.Inst.In(this);  // Ǯ�ȿ� �������

        if (uiManager.Get_MixButtonOn && transform.position.y < 0.5f)
        {
            transform.position = new Vector3(transform.position.x + 1f, transform.position.y + 0.1f, -2);
        }
        else
        {
            if (transform.position.y > moveObj.transform.position.y)
            {
                transform.position = new Vector3(transform.position.x + 0.7f, transform.position.y - 0.07f, -2);
            }
            if (transform.position.y < moveObj.transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, moveObj.transform.position.y, -2);
            }
        }
    }
}
