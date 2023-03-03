using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartDirection : MonoBehaviour
{
    [SerializeField] private GameObject ui = null;
    [SerializeField] private Image orderList = null;
    private bool receiptOn = false;

    public float moveSpeed = 70f;

    private void Start()
    {
        StartCoroutine(DirectionOn());
    }

    WaitForSeconds wait0001 = new WaitForSeconds(0.001f);

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

    private IEnumerator ReceiptOn()
    {
        RectTransform rt = orderList.GetComponent<RectTransform>();

        while (rt.localPosition.x < 0)
        {
            rt.localPosition = new Vector3(rt.localPosition.x + (Time.deltaTime * 1500f), rt.localPosition.y, 0);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);
        GameManager.Inst.Set_DirectionWait(false);
    }
}
