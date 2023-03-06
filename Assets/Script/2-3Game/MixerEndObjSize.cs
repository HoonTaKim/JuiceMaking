using System.Collections;
using UnityEngine;
using TMPro;

public class MixerEndObjSize : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI juiceName = null;
    [SerializeField] private GameObject particle = null;
    
    void Start()
    {
        // 게임 매니저에 저장되어있는 주스 이미지, 이름으로 변경
        this.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GameManager.Inst.saveOrderImage;
        juiceName.text = GameManager.Inst.saveOrderName;

        juiceName.gameObject.SetActive(true);
        particle.SetActive(true);
        StartCoroutine(Size());
    }

    // 사이즈가 점점 커지는 코루틴
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

    // 비활성화시 파티클 비활성화
    private void OnDisable()
    {
        particle.SetActive(false);
    }
}
