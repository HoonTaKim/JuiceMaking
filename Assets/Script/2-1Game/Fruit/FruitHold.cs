using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class FruitHold : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private List<Image> fruitList = new List<Image>();          // ID��ũ��Ʈ�� �߰�

    [SerializeField] SelectFruit_UiManager uiManager = null;    
    private FruitCase fruitCase = null;                         // ������ ���� ���� ����

    [Header("CanvasRayCast")]
    [SerializeField] private Canvas canvas = null;
    private GraphicRaycaster raycast;

    [Header("�� ����")]
    //[SerializeField] private JuiceColor jc = null;              // �ſ� �� ���Ͽ� ���� �ٲ�� ���� ������ ����
    [SerializeField] private FruitCase leftCase = null;         // ���� ��
    [SerializeField] private FruitCase rightCase = null;        // ������ ��

    [Header("���� ����")]
    [SerializeField] private Transform moveParent = null;       // ������ �̵��ϴµ��� �� �θ� ������Ʈ
    private Image fruitSelect = null;                           // ������ �̹����� ������ ����
    private RectTransform fruitRt = null;                       // ������ ��ġ�� ������ ����
    private Transform fruitParent = null;                       // ������ �θ� �����ų ����


    private void Start()
    {
        Init();
    }

    private void Init()
    {
        raycast = canvas.GetComponent<GraphicRaycaster>();
    }

    // �巡�׽� ó�� �ѹ� ȣ��
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (uiManager.Get_StorageMoveOn) return;

        SoundManager.Inst.PlaySFX("Putting fruits.");

        // Ui RayCast ����
        List<RaycastResult> results = new List<RaycastResult>();
        Debug.DrawRay(eventData.position, Vector3.forward, Color.red, 500f);
        raycast.Raycast(eventData, results);

        // RayCast�� ����Ȱ͵��� FruitID, FruitCase ã��
        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.TryGetComponent<FruitID>(out FruitID fruit))
            {
                // �����Ѵٸ� ������ ��ġ�� �θ� ����
                fruitSelect = fruit.gameObject.GetComponent<Image>();
                fruitRt = fruit.Get_SaveTr;
                fruitParent = fruit.Get_Parent;
            }
            if (results[i].gameObject.TryGetComponent<FruitCase>(out FruitCase cup))
            {
                fruitCase = cup;
            }
        }

        if (fruitCase != null) fruitCase.Set_CaseOn(false);
        if (fruitSelect == null) return;
        else
        {
            StartCoroutine(SelectCatchScale());
        }
        if (fruitSelect.GetComponent<FruitID>().Get_CaseIn) fruitSelect = null;
    }

    // �����ϵ� ��ġ�ʱ�ȭ
    public void FruitListTransform()
    {
        if (fruitList.Count <= 0) return;

        for (int i = 0; i < fruitList.Count; i++)
        {
            // �ݺ����� ���� ���ϵ��� ������ ��ġ�� �̵�
            FruitID fruitId = fruitList[i].GetComponent<FruitID>();
            fruitId.ReSetTR_ButtonOn(false);
            fruitList[i].rectTransform.position = new Vector3(fruitId.Get_SaveTr.position.x, fruitId.Get_SaveTr.position.y, fruitId.Get_SaveTr.position.z);
            fruitList[i].transform.parent = fruitId.Get_Parent;
            GameManager.Inst.CaseInFruit(-1);
        }

        leftCase.CountChange(2);
        rightCase.CountChange(2);
        GameManager.Inst.CaseInFruit(-100);

        fruitList.Clear();
    }

    // �巡���ϴ� ���� ���콺 Ŀ���� ������ ������ ȣ��
    public void OnDrag(PointerEventData eventData)
    {
        if (fruitSelect == null) return;

        // �巡�׵��� ��ġ ����Ʈ�� ���󰡸� �θ�� moveParent�� �̵�
        fruitSelect.rectTransform.position = eventData.position;
        fruitSelect.transform.SetParent(moveParent);
    }

    /// <summary>
    /// Ư�� ������ �����ϰ� �ִ� ���� �ش� ������ localScale�� Ŀ���� ������ִ��ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator SelectCatchScale()
    {
        while (fruitSelect.rectTransform.localScale.x <= 1.5f)
        {
            fruitSelect.rectTransform.localScale = new Vector2(fruitSelect.rectTransform.localScale.x + 0.1f, fruitSelect.rectTransform.localScale.y + 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        fruitSelect.rectTransform.localScale = new Vector2(1.5f, 1.5f);
    }

    // �巡�� ����ÿ� �ѹ� ȣ��
    public void OnEndDrag(PointerEventData eventData)
    {
        if (fruitSelect == null) return;
        Image hitCase = null;
        ScrollRect hitCaseChild = null;

        fruitSelect.rectTransform.localScale = new Vector2(1f, 1f);

        List<RaycastResult> results = new List<RaycastResult>();
        raycast.Raycast(eventData, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.TryGetComponent<FruitCase>(out FruitCase obj))
            {
                hitCase = obj.gameObject.GetComponent<Image>();
                hitCaseChild = obj.gameObject.transform.GetChild(0).GetComponent<ScrollRect>();
            }
        }

        if (hitCase == null)
        {
            fruitSelect.transform.SetParent(fruitParent);
            fruitSelect.rectTransform.position = fruitRt.position;
        }


        if (hitCase != null)
        {
            SoundManager.Inst.PlaySFX("Putting fruits.");

            //���콺�� �� ������ select�� ��� �ִ� ������ ��� �־���, ���� �ٱ��Ͽ� �־���.
            FruitID fruit = fruitSelect.GetComponent<FruitID>();
            FruitCase fruitCase = hitCase.GetComponent<FruitCase>();

            //fruit.get_id�� ���� �� �ſ� �� ������ �� �� �ִ��� Ȯ�� �ϰ�, ������ �� ������ ��
            if (fruit.Get_Id == fruitCase.Get_CaseId && fruitCase.Get_CupCount > 0)
            {
                SoundManager.Inst.PlaySFX("AnswerFruit");

                fruitSelect.transform.SetParent(hitCaseChild.content.transform);
                fruitSelect.rectTransform.position = hitCaseChild.content.transform.position;
                fruit.FruitCaseIn(fruitCase);

                fruitCase.CountChange(-1);
                fruit.ReSetTR_ButtonOn(true);

                fruitList.Add(fruitSelect);
                GameManager.Inst.CaseInFruit(1);
            }
            else
            {
                SoundManager.Inst.PlaySFX("WrongFruit");

                fruitSelect.transform.SetParent(fruitParent);
                fruitSelect.rectTransform.position = fruitRt.position;
                GameManager.Inst.Get_SelectFruitUiManager.WrongTextImageOn(fruitCase.transform);
            }
        }
        else
        {
            fruitSelect.transform.SetParent(fruitParent);
            fruitSelect.rectTransform.position = fruitRt.position;
        }

        fruitSelect = null;
    }
}