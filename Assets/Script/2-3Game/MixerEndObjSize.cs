using System.Collections;
using UnityEngine;
using TMPro;

public class MixerEndObjSize : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI juiceName = null;
    [SerializeField] private GameObject particle = null;
    
    void Start()
    {
        // ���� �Ŵ����� ����Ǿ��ִ� �ֽ� �̹���, �̸����� ����
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GameManager.Inst.saveOrderImage;
        juiceName.text = GameManager.Inst.saveOrderName;

        juiceName.gameObject.SetActive(true);
        particle.SetActive(true);
        StartCoroutine(Size());
    }

    // ����� ���� Ŀ���� �ڷ�ƾ
    IEnumerator Size()
    {
        while (this.transform.localScale.x < 1)
        {
            this.transform.localScale = new Vector3(this.transform.localScale.x + 0.01f, this.transform.localScale.y + 0.01f, 1);
            juiceName.rectTransform.localPosition = new Vector3(0, juiceName.rectTransform.localPosition.y - 3f, 0);
            yield return new WaitForSeconds(0.001f);
        }

        this.transform.localScale = new Vector3(1,1,1);
        juiceName.rectTransform.localPosition = new Vector3(0, -290f, 0);
    }

    // ��Ȱ��ȭ�� ��ƼŬ ��Ȱ��ȭ
    private void OnDisable()
    {
        particle.SetActive(false);
    }
}
