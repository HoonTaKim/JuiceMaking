using UnityEngine;
using UnityEngine.UI;

public class FruitID : MonoBehaviour
{
    private RectTransform saveTr = null;    // �ʱ���ġ�� ������ ����
    private Transform parent = null;        // �ʱ�θ� ������ ����
    private Button button = null;           // ��ġ �ʱ�ȭ�Ҷ� ����� ����

    public int id = 0;                      // ������ id
    [SerializeField] private bool caseIn = false;   // �� ������ �̵��ߴ��� 

    private FruitCase fruitCase = null;

    public RectTransform Get_SaveTr { get { return saveTr; } private set { } }
    public Transform Get_Parent { get { return parent; } private set { } }
    public bool Get_CaseIn { get { return caseIn; } private set { } }
    public int Get_Id { get { return id; } private set { } }

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    void Start()
    {
        //���ư� ��ġ, �θ� ����
        saveTr = transform.parent.GetComponent<Image>().rectTransform;
        parent = transform.parent;
    }

    // ���� ������ �� ���� ���� ����
    public void FruitCaseIn(FruitCase _fruitCase)
    {
        fruitCase = _fruitCase;
    }

    // �žȿ� ������ Ȥ�� �������� ��ư Ȱ��ȭ, ��Ȱ��ȭ
    public void ReSetTR_ButtonOn(bool _trriger)
    {
        button.enabled = _trriger;
    }

    //�߸��� ���� �־����� ��ġ �ʱ�ȭ���ִ� �Լ�.
    public void OnClick_TransformReset()
    {
        this.transform.parent = parent.transform;
        this.transform.position = saveTr.position;
        fruitCase.CountChange(1);
        GameManager.Inst.CaseInFruit(-1);
        fruitCase = null;
        ReSetTR_ButtonOn(false);
    }
}
