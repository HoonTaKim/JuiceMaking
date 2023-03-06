using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartDirection : MonoBehaviour
{
    [SerializeField] private GameObject ui = null;
    [SerializeField] private Image orderList = null;    // 주문서 리스트
    private bool receiptOn = false;

    private float moveSpeed = 1500f;

    private void Start()
    {
        StartCoroutine(DirectionOn());
    }

    // Scale값을 조절해 카메라가 점점 축소되는듯한 연출
    private IEnumerator DirectionOn()
    {
        GameManager.Inst.Set_DirectionWait(true);

        while (ui.transform.localScale.x >= 1)
        {
            ui.transform.localScale = new Vector3(ui.transform.localScale.x - Time.deltaTime, ui.transform.localScale.y - Time.deltaTime, 1) ;
            yield return null;

            if (ui.transform.localScale.x >= 0.5 && !receiptOn)
            {
                StartCoroutine(ReceiptOn());
                receiptOn = true;
            }
        }

        ui.transform.localScale = new Vector3(1, 1, 1);
    }

    // 주문서들을 화면 중심으로 이동시키는 코루틴
    private IEnumerator ReceiptOn()
    {
        RectTransform rt = orderList.GetComponent<RectTransform>();

        while (rt.localPosition.x < 0)
        {
            rt.localPosition = new Vector3(rt.localPosition.x + (Time.deltaTime * moveSpeed), rt.localPosition.y, 0);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        GameManager.Inst.Set_DirectionWait(false);
    }
}
