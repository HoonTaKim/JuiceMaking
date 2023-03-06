using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixImageMove : MonoBehaviour
{
    private MixImage2Move moveObj = null;   // �̵��ϴ� �ֽ��� �̹���
    private UiManager uiManager = null;     // UiManager
    private bool createOn = false;          // �ѿ�����Ʈ�� �ϳ��� ������ų �� �ֵ��� �����ϴ� bool��

    private void Awake()
    {
        uiManager = FindObjectOfType<UiManager>();
        moveObj = GameManager.Inst.Get_MixerStart.movePos.GetComponent<MixImage2Move>();
    }

    // Ȱ��ȭ�� ���� �Ŵ����� ����Ǿ��ִ� �ֽ��� ������ ����
    private void OnEnable()
    {
        transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(GameManager.Inst.saveJuiceColor.r, GameManager.Inst.saveJuiceColor.g, GameManager.Inst.saveJuiceColor.b);
        transform.GetChild(1).GetComponent<SpriteRenderer>().color = new Color(GameManager.Inst.saveJuiceColor.r, GameManager.Inst.saveJuiceColor.g, GameManager.Inst.saveJuiceColor.b);
        transform.GetChild(2).GetComponent<SpriteRenderer>().color = new Color(GameManager.Inst.saveJuiceColor.r, GameManager.Inst.saveJuiceColor.g, GameManager.Inst.saveJuiceColor.b);
        createOn = false;
    }

    private void Update()
    {
        // �������� �̵��ϴ� �ֽ��� �̹����� Ư�� ��ġ�� ����������
        // ���ο� �ֽ��� �̹����� Ȱ��ȭ
        if (transform.position.x >= 12f && !createOn)
        {
            MixPooling.Inst.Out(transform.position.y);
            createOn = true;
        }

        // �������� �̵��ϴ� �ֽ��� �̹����� Ư�� ��ġ�� ����������
        // ���� ������Ʈ�� ������Ʈ Ǯ ������ �̵�
        if (transform.position.x > 15f) MixPooling.Inst.In(this);  // Ǯ�ȿ� �������

        if (uiManager.Get_MixButtonOn && transform.position.y < 0.5f)
        {
            // ��ư�� �������� �׸��� ���� ���� �̻����� �ö�����
            // �ֽ��� �̹����� �������� �̵��ϸ� ���� ���
            transform.position = new Vector3(transform.position.x + (Time.deltaTime * 10f), transform.position.y + (Time.deltaTime * 5.5f), -2);
        }
        else
        {
            // �̿ܿ� �������� �̵��ϴ� �ֽ� �̹����� �������� �̵��ϸ� ���� �Ʒ������� �̵�
            if (transform.position.y > moveObj.transform.position.y)
            {
                transform.position = new Vector3(transform.position.x + (Time.deltaTime * 70f), transform.position.y - (Time.deltaTime * 7f), -2);
            }
            if (transform.position.y < moveObj.transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, moveObj.transform.position.y, -2);
            }
        }
    }
}
