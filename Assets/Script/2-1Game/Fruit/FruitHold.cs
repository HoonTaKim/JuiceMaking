using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class FruitHold : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private List<Image> fruitList = new List<Image>();        // ID��ũ��Ʈ�� �߰�



    [SerializeField] SelectFruit_UiManager uiManager = null;
    private FruitCase fruitCase = null;
    private Color color;
    private bool caseIn = false;
    private int caseId = 0;

    [Header("CanvasRayCast")]
    [SerializeField] private Canvas canvas = null;
    private GraphicRaycaster raycast;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;

    [SerializeField] private FruitCase leftCase = null;
    [SerializeField] private FruitCase rightCase = null;
    [SerializeField] private Transform moveParent = null;
    Image select = null;
    RectTransform rt = null;
    Transform parent = null;

    [SerializeField] private JuiceColor jc = null;

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

        List<RaycastResult> results = new List<RaycastResult>();
        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = eventData.position;
        Debug.DrawRay(pointerEventData.position, Vector3.forward, Color.red, 500f);
        raycast.Raycast(pointerEventData, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.TryGetComponent<FruitID>(out FruitID obj))
            {
                select = obj.gameObject.GetComponent<Image>();
                rt = obj.Get_SaveTr;
                parent = obj.Get_Parent;
            }
            if (results[i].gameObject.TryGetComponent<FruitCase>(out FruitCase obj2))
            {
                fruitCase = obj2;
            }
        }

        if (fruitCase != null) fruitCase.Set_CaseOn(false);
        if (select == null) return;
        else
        {
            StartCoroutine(SelectCatchScale());
        }
        if (select.GetComponent<FruitID>().Get_CaseIn) select = null;
    }

    // �����ϵ� ��ġ�ʱ�ȭ
    public void FruitListTransform()
    {
        if (fruitList.Count <= 0) return;

        for (int i = 0; i < fruitList.Count; i++)
        {
            FruitID fruitId = fruitList[i].GetComponent<FruitID>();
            Debug.Log(fruitList[i].transform.parent.transform.parent);
            //fruitList[i].
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
        if (select == null) return;

        select.rectTransform.position = eventData.position;
        select.transform.SetParent(moveParent);
    }

    /// <summary>
    /// Ư�� ������ �����ϰ� �ִ� ���� �ش� ������ localScale�� Ŀ����?������ִ��ڷ�ƾ
    /// </summary>
    /// <returns></returns>
    private IEnumerator SelectCatchScale()
    {
        while (select.rectTransform.localScale.x <= 1.5f)
        {
            select.rectTransform.localScale = new Vector2(select.rectTransform.localScale.x + 0.1f, select.rectTransform.localScale.y + 0.1f);
            yield return new WaitForSeconds(0.01f);
        }
        select.rectTransform.localScale = new Vector2(1.5f, 1.5f);
    }

    // �巡�� ����ÿ� �ѹ� ȣ��
    public void OnEndDrag(PointerEventData eventData)
    {
        if (select == null) return;

        select.rectTransform.localScale = new Vector2(1f, 1f);

        pointerEventData = new PointerEventData(eventSystem);
        pointerEventData.position = eventData.position;
        List<RaycastResult> results = new List<RaycastResult>();
        Image hitCase = null;
        ScrollRect hitCaseChild = null;
        raycast.Raycast(pointerEventData, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.TryGetComponent<FruitCase>(out FruitCase obj))
            {
                hitCase = obj.gameObject.GetComponent<Image>();
                hitCaseChild = obj.gameObject.transform.GetChild(0).GetComponent<ScrollRect>();
            }
            if (results[i].gameObject.TryGetComponent<FruitID>(out FruitID fruitObj))
            {
                if (fruitObj != select.GetComponent<FruitID>())
                {
                    caseIn = true;
                }
            }
        }

        if (hitCase == null)
        {
            select.transform.SetParent(parent);
            select.rectTransform.position = rt.position;
        }


        if (hitCase != null)
        {
            SoundManager.Inst.PlaySFX("Putting fruits.");

            //���콺�� �� ������ select�� ��� �ִ� ������ ��� �־���, ���� �ٱ��Ͽ� �־���.
            FruitID fruit = select.GetComponent<FruitID>();
            FruitCase fruitCase = hitCase.GetComponent<FruitCase>();

            //fruit.get_id�� ���� �� �ſ� �� ������ �� �� �ִ��� Ȯ�� �ϰ�, ������ �� ������ ��
            if (!caseIn && fruit.Get_Id == fruitCase.Get_CaseId && fruitCase.Get_CupCount > 0)
            {
                SoundManager.Inst.PlaySFX("AnswerFruit");

                select.transform.SetParent(hitCaseChild.content.transform);
                select.rectTransform.position = hitCaseChild.content.transform.position;
                fruit.FruitCaseIn(fruitCase);

                Debug.Log(hitCase.sprite.name);

                ColorSeting(hitCase.sprite.name);
                fruitCase.CountChange(-1);
                fruit.ButtonOn(true);

                jc.ChangeColor(color);
                fruitList.Add(select);
                GameManager.Inst.CaseInFruit(1);
            }
            else
            {
                SoundManager.Inst.PlaySFX("WrongFruit");

                select.transform.SetParent(parent);
                select.rectTransform.position = rt.position;
                GameManager.Inst.Get_SelectFruitUiManager.WrongTextImageOn(fruitCase.transform);
            }
        }
        else
        {
            select.transform.SetParent(parent);
            select.rectTransform.position = rt.position;
        }

        select = null;
        caseIn = false;
    }

    public void ColorSeting(string _cupColor)
    {
        switch (_cupColor)
        {
            case "Cup_Red":
                break;
            case "Cup_Orange":
                break;
            case "Cup_Yellow":
                break;
            case "Cup_Green":
                break;
            case "Cup_Blue":
                break;
            case "Cup_Puple":
                break;
            case "Cup_White":
                break;
        }
    }
}