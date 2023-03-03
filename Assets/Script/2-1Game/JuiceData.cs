using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//[System.Serializable]
//public class JuiceInfo
//{
//    public string juiceName;
//    public Sprite juiceImage;


//    public Sprite leftColorCup_Image = null;

//    public Sprite rightColorCup_Image = null;
//}

public class JuiceData : MonoBehaviour
{
    private bool setComplete = false;

    [Header("���� ������")]
    [SerializeField] private int fruitCount = 0;          // ������ ��ü ����
    [SerializeField] private Transform storageParent = null;            // ������ �ʱ� ��ġ
    [SerializeField] private Image fruitPrefab = null;                  // ���� ������
    //[SerializeField] private List<Sprite> sliceTextuerList = null;      // �߸� ���� �̹��� ����Ʈ
    private List<Vector2> fruitPosList = new List<Vector2>();           // ���� �ٱ��Ͼ� ������ ���� ��ġ ����Ʈ
    private List<Sprite> saveFruit_List = new List<Sprite>();           // ��� ���ϵ��� ��������Ʈ�� ������ ����Ʈ
    public List<string> fruit_DataList = new List<string>();            // csv�� ���� ���� ���� ��������Ʈ ���� ����Ʈ
    public List<Sprite> leftCupFruit_Image = new List<Sprite>();
    public List<Sprite> rightCupFruit_Image = new List<Sprite>();

    //[SerializeField] private List<JuiceInfo> fruitInfoList = null;      // CSV�� ���� �޾ƿ��⶧���� ���� ����

    [Header("�� ������")]
    [SerializeField] private Image leftCup = null;                      // ���� �� ����
    [SerializeField] private Image rightCup = null;                     // ������ �� ����
    public List<string> cup_DataList = new List<string>();

    [Header("�ֹ��� ������")]
    [SerializeField] private List<RawImage> orderSheetList = new List<RawImage>();      // �ֹ��� ����
    public List<string> orderSheet_DataList = new List<string>();                       // csv�� ���� ���� �ֹ��� ���� ����Ʈ


    int leftCup_Count = 0;
    int leftCup_Id = 0;

    int rightCup_Count = 0;
    int rightCup_Id = 0;

    #region ������Ƽ
    public List<Sprite> Get_SaveFruit_List { get { return saveFruit_List; } private set { } }
    #endregion

    private void Start()
    {
        orderSheet_DataList = CSVLoad.LoadData("JuiceMaking_OrderDataSheet");
        fruit_DataList = CSVLoad.LoadData("JuiceMaking_FruitData");
        cup_DataList = CSVLoad.LoadData("JuiceMaking_CupData");

        NullDataDel(orderSheet_DataList);
        NullDataDel(fruit_DataList);
        NullDataDel(cup_DataList);

        OrderSheetSeting();

        // ���� ��ġ ����
        fruitPosList.Add(new Vector2(-20, 230));
        fruitPosList.Add(new Vector2(130, 230));
        fruitPosList.Add(new Vector2(-20, 70));
        fruitPosList.Add(new Vector2(130, 70));
        fruitPosList.Add(new Vector2(-20, -100));
        fruitPosList.Add(new Vector2(130, -100));
        fruitPosList.Add(new Vector2(-20, -260));
        fruitPosList.Add(new Vector2(130, -260));
    }

    public void OrderSheetSeting()
    {
        string[] data;

        while (orderSheetList.Count != orderSheet_DataList.Count)
        {
            orderSheet_DataList.Remove(orderSheet_DataList[Random.Range(0, orderSheet_DataList.Count)]);
        }

        for (int i = 0; i < orderSheetList.Count; i++)
        {
            data = orderSheet_DataList[i].Split(",");
            orderSheetList[i].GetComponent<RawImage>().texture = Resources.Load<Texture>(data[7]);
            orderSheetList[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = data[1];
        }
    }

    // �ֹ����� ���� �ֽ��� �̸��� ���� ���ϰ� ���� ������ �Է��ϴ� �Լ�
    public void JuiceIngredientSeting(string _juiceName)
    {

        if (setComplete) return;

        // ������ RectTransform�� Pos�� �������� �ۼ��� + �θ� �����ص� ���·� ������
        // �ٱ����� �ʺ�� 1270 ~ 1490, ���̴� -330 ~ 330
        // �ٱ����� �ʺ�� -42 ~ 130, ���̴� -300 ~ 300
        // �ٱ����� �ʺ�� -20 ~ 130, ���̴� -290 ~ 280 
        // �������� 1 ������ ���� 2 �ش������ 0

        string[] orderData = new string[0];

        for (int i = 0; i < orderSheet_DataList.Count; i++)
        {
            orderData = orderSheet_DataList[i].Split(",");
            if (_juiceName == orderData[1])
            {
                break;
            }
        }

        GameManager.Inst.saveOrderImage = Resources.Load<Sprite>("Image/JuiceImage/" + orderData[7] + "_Result");
        GameManager.Inst.saveOrderName = orderData[1];

        FruitCase leftCase = leftCup.GetComponent<FruitCase>();
        FruitCase rightCase = rightCup.GetComponent<FruitCase>();

        leftCup_Count = int.Parse(orderData[3]);
        leftCup_Id = int.Parse(orderData[2]);

        rightCup_Count = int.Parse(orderData[5]);
        rightCup_Id = int.Parse(orderData[4]);

        AddFruit(orderData, 2, leftCupFruit_Image, true);
        AddFruit(orderData, 4, rightCupFruit_Image, false);

        // ���� ���ھ ���ʰ� ������ ���� ������ ���� ���
        GameManager.Inst.Set_MaxScore(leftCup_Count + rightCup_Count);

        // �����Ű� ���������� �Է°����� ���� �Է�
        leftCase.SetCupMaxCount(leftCup_Count);
        rightCase.SetCupMaxCount(rightCup_Count);

        // �����Ű� �������ſ� ID�� �ο�
        leftCase.case_Id = leftCup_Id;
        rightCase.case_Id = rightCup_Id;

        //�� ���� Color �� Sprite �ֱ�
        leftCup.sprite = CupSpriteSeting(leftCase.case_Id);
        leftCase.color = getCupColorFromSprite(leftCup.sprite);
        leftCase.isRight = false;

        rightCup.sprite =  CupSpriteSeting(rightCase.case_Id);
        rightCase.color = getCupColorFromSprite(rightCup.sprite);
        rightCase.isRight = true;

        // ���� ����
        AnswerFruintSeting(leftCup_Count, leftCupFruit_Image, leftCup_Id, true);                        // ���� �ſ� �� ���ϵ�
        AnswerFruintSeting(rightCup_Count, rightCupFruit_Image, rightCup_Id, true);                     // ������ �� �� ���ϵ�
        AnswerFruintSeting(fruitCount - (leftCup_Count + rightCup_Count), saveFruit_List, 0, false);    // �̿��� ���ϵ�

        Debug.Log("��??????????????��?????????????????????");
        setComplete = true;
    }

    public void NullDataDel(List<string> dataList)
    {
        for (int i = dataList.Count - 1; i > 0; i--)
        {
            if (dataList[i] == null)
            {
                dataList.Remove(dataList[i]);
            }
            else break;
        }
    }

    public void AddFruit(string[] orderData, int idx, List<Sprite> spriteList, bool chack)
    {
        string[] tempArr;
        for (int i = 0; i < fruit_DataList.Count; i++)
        {
            tempArr = fruit_DataList[i].Split(",");

            if (chack)
            {
                saveFruit_List.Add(Resources.Load<Sprite>("Image/Fruit/" + tempArr[1]));
            }

            if (orderData[idx] == tempArr[2])
            {
                spriteList.Add(Resources.Load<Sprite>("Image/Fruit/" + tempArr[1]));
                saveFruit_List.Remove(Resources.Load<Sprite>("Image/Fruit/" + tempArr[1]));
            }
        }
    }

    //�����Ҽ��������غ���
    // �� ���� �Է�
    FRUITCOLOR getCupColorFromSprite(Sprite sp)
    {
        FRUITCOLOR color = FRUITCOLOR.DEFAULT;

        switch (sp.name)
        {
            case "Cup_Blue":
                color = FRUITCOLOR.BLUE;
                break;
            case "Cup_Green":
                color = FRUITCOLOR.GREEN;
                break;
            case "Cup_Orange":
                color = FRUITCOLOR.ORANGE;
                break;
            case "Cup_Puple":
                color = FRUITCOLOR.PURPLE;
                break;
            case "Cup_Red":
                color = FRUITCOLOR.RED;
                break;
            case "Cup_White":
                color = FRUITCOLOR.WHITE;
                break;
            case "Cup_Yellow":
                color = FRUITCOLOR.YELLOW;
                break;
        }

        if(color == FRUITCOLOR.DEFAULT)
        {
            Debug.Log("�����߸����");
        }    

        return color;
    }

    // �� ��������Ʈ �Է�
    public Sprite CupSpriteSeting(int id)
    {
        string[] data;
        Sprite sp = null;

        switch (id)
        {
            case 1:
                // ����
                data = cup_DataList[0].Split(",");
                sp = Resources.Load<Sprite>(data[1]);
                break;
            case 2:
                // ��Ȳ
                data = cup_DataList[1].Split(",");
                sp = Resources.Load<Sprite>(data[1]);
                break;
            case 3:
                // ���
                data = cup_DataList[2].Split(",");
                sp = Resources.Load<Sprite>(data[1]);
                break;
            case 7:
                // �Ķ�
                data = cup_DataList[3].Split(",");
                sp = Resources.Load<Sprite>(data[1]);
                break;
            case 5:
                // �ʷ�
                data = cup_DataList[4].Split(",");
                sp = Resources.Load<Sprite>(data[1]);
                break;
            case 8:
                // ����
                data = cup_DataList[5].Split(",");
                sp = Resources.Load<Sprite>(data[1]);
                break;
            case 10:
                // �Ͼ�
                data = cup_DataList[6].Split(",");
                sp = Resources.Load<Sprite>(data[1]);
                break;
        }
        return sp;
    }

    // ������ _list�� �����ų �����͸� �������ִ� _addList
    public void SaveTexture(List<Sprite> _list, List<Sprite> _addList)
    {
        for (int i = 0; i < _addList.Count; i++)
        {
            _list.Add(_addList[i]);
        }
    }

    public void SaveSlice(List<Sprite> _list, List<Sprite> _addList, List<Sprite> _chackList, string _number)
    {
        for (int i = 0; i < _addList.Count; i++)
        {
            for (int j = 0; j < _chackList.Count; j++)
            {
                if (_addList[i].name == _chackList[j].name + _number)
                {
                    _list.Add(_addList[i]);
                }
            }
        }
    }

    // ������ ��ġ�� ���� ������Ű�� �Լ�
    // ������ ������ ����, ������ų ������ ��������Ʈ ����Ʈ, ���� ID, �žȿ� ���°������� �ƴ���
    public void AnswerFruintSeting(int _fruitNum, List<Sprite> _fruitSpriteList, int _id, bool _dirListOn)
    {
        Image fruit = null;
        int posIdx = 0;
        int fruitIdx = 0;

        for (int i = 0; i < _fruitNum; i++)
        {
            // ���� id���� ���� ������ �������� ���
            fruitIdx = Random.Range(0, _fruitSpriteList.Count);
            fruit = Instantiate(fruitPrefab);
            fruit.transform.GetChild(0).GetComponent<Image>().sprite = _fruitSpriteList[fruitIdx];
            fruit.transform.GetChild(0).GetComponent<FruitID>().id = _id;

            if (_dirListOn)
            {
                GameManager.Inst.Get_FruitSpriteList.Add(_fruitSpriteList[fruitIdx]);
                GameManager.Inst.Get_SliceSpriteList.Add(Resources.Load<Sprite>("Image/SliceFruit/" + _fruitSpriteList[fruitIdx].name + "1"));
                GameManager.Inst.Get_SliceSpriteList.Add(Resources.Load<Sprite>("Image/SliceFruit/" + _fruitSpriteList[fruitIdx].name + "2"));
            }
            _fruitSpriteList.Remove(_fruitSpriteList[fruitIdx]);

            if (fruitPosList.Count == 1)
                posIdx = 0;
            else
                posIdx = Random.Range(0, fruitPosList.Count - 1);

            fruit.rectTransform.parent = storageParent;
            fruit.rectTransform.localPosition = fruitPosList[posIdx];
            fruit.rectTransform.localPosition = new Vector2(fruit.rectTransform.localPosition.x + Random.Range(-35f, 35f), fruit.rectTransform.localPosition.y + Random.Range(-35f, 35f));
            fruitPosList.Remove(fruitPosList[posIdx]);
        }
    }
}
