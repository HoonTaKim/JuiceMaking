using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FruitID : MonoBehaviour
{
    private RectTransform saveTr = null;
    private Transform parent = null;
    private Button button = null;

    public int id = 0;
    [SerializeField] private bool caseIn = false;

    private FruitCase fruitCase = null;

    public RectTransform Get_SaveTr { get { return saveTr; } private set { } }
    public Transform Get_Parent { get { return parent; } private set { } }
    public bool Get_CaseIn { get { return caseIn; } private set { } }
    public int Get_Id { get { return id; } private set { } }

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //���ư� ��ġ ����
        saveTr = transform.parent.GetComponent<Image>().rectTransform;
        parent = transform.parent;
    }

    public void FruitCaseIn(FruitCase _fruitCase)
    {
        fruitCase = _fruitCase;
    }

    public void ButtonOn(bool _trriger)
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

        //���߿��������ּ�������

        fruitCase = null;
        ButtonOn(false);
    }

    public bool SetID(int _caseID)
    {
        if (id == _caseID) caseIn = true;

        return caseIn;
    }
}
