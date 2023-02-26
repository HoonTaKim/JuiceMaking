using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartDirection : MonoBehaviour
{
    [SerializeField] private GameObject ui = null;
    [SerializeField] private List<RawImage> receiptList = new List<RawImage>();
    [SerializeField] private Image orderList = null;
    private bool receiptOn = false;

    private void Start()
    {
        StartCoroutine(DirectionOn());
    }

    private IEnumerator DirectionOn()
    {
        GameManager.Inst.Set_DirectionWait(true);

        while (ui.transform.localScale.x >= 1)
        {
            ui.transform.localScale = new Vector3(ui.transform.localScale.x - 0.01f, ui.transform.localScale.y - 0.01f, 1);
            yield return new WaitForSeconds(0.01f);

            if (ui.transform.localScale.x >= 0.5 && !receiptOn)
            {
                StartCoroutine(ReceiptOn());
                receiptOn = true;
            }
        }

        ui.transform.localScale = new Vector3(1, 1, 1);
    }

    private IEnumerator ReceiptOn()
    {
        RectTransform rt = orderList.GetComponent<RectTransform>();

        while (rt.localPosition.x < 0)
        {
            rt.localPosition = new Vector3(rt.localPosition.x + 30f, rt.localPosition.y, 0);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.5f);
        GameManager.Inst.Set_DirectionWait(false);
    }
}
