using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// ���� ����� ������ �޸� �ֹ��� ��ũ��Ʈ
public class ReceipInfo : MonoBehaviour
{
    RectTransform rt = null;

    private void Awake()
    {
        rt = GetComponent<RectTransform>();
    }

    // �ʱ�ȭ �Լ�
    public void Init(Texture _texture, string _name)
    {
        GetComponent<RawImage>().texture = _texture;
        transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = _name;

        StartCoroutine(ScaleOn());
    }

    // �������� ������ �۾����� Ŀ���� ���� �ڷ�ƾ
    IEnumerator ScaleOn()
    {
        while (rt.localScale.x <= 1.1f)
        {
            rt.localScale = new Vector3(rt.localScale.x + 0.05f,
                                        rt.localScale.y + 0.05f, 0);
            yield return new WaitForSeconds(0.01f);
        }
        while (rt.localScale.x >= 0.95f)
        {
            rt.localScale = new Vector3(rt.localScale.x - 0.05f, rt.localScale.y - 0.05f, 0);
            yield return new WaitForSeconds(0.01f);
        }
        while (rt.localScale.x >= 1f)
        {
            rt.localScale = new Vector3(rt.localScale.x + 0.05f, rt.localScale.y + 0.05f, 0);
            yield return new WaitForSeconds(0.01f);
        }
        rt.localScale = new Vector3(1, 1, 0);
    }
}
