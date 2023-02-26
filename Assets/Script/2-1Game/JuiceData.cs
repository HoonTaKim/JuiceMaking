using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class JuiceInfo
{
    public string juiceName;
    public Sprite juiceImage;

    public int fruitCount = 0;          // ������ ��ü ����

    public int leftCupCount = 0;
    public Sprite leftColorCup_Image = null;
    public List<Sprite> leftCupFruit_Image = new List<Sprite>();

    public int rightCupCount = 0;
    public Sprite rightColorCup_Image = null;
    public List<Sprite> rightCupFruit_Image = new List<Sprite>();
}

public class JuiceData : MonoBehaviour
{
    [SerializeField] private Transform storageParent = null;
    [SerializeField] private Image fruitPrefab = null;
    [SerializeField] private List<Sprite> fruitTextuerList = null;
    [SerializeField] private List<Sprite> sliceTextuerList = null;
    [SerializeField] private List<JuiceInfo> fruitInfoList = null;
    private List<Sprite> wrongFruitInfoList = null;
    [SerializeField] private Image leftCup = null;
    [SerializeField] private Image rightCup = null;

    private List<Vector2> fruitPosList = new List<Vector2>();
    private List<Sprite> saveFruitTextuerList = new List<Sprite>();

    public List<Sprite> Get_SaveFruitTextuerList { get { return saveFruitTextuerList; } private set { } }

    private void Start()
    {
        wrongFruitInfoList = new List<Sprite>();

        fruitPosList.Add(new Vector2(-20, 230));
        fruitPosList.Add(new Vector2(130, 230));
        fruitPosList.Add(new Vector2(-20, 70));
        fruitPosList.Add(new Vector2(130, 70));
        fruitPosList.Add(new Vector2(-20, -100));
        fruitPosList.Add(new Vector2(130, -100));
        fruitPosList.Add(new Vector2(-20, -260));
        fruitPosList.Add(new Vector2(130, -260));
    }

    public void JuiceIngredientSeting(string _juiceName)
    {
        // ������ RectTransform�� Pos�� �������� �ۼ��� + �θ� �����ص� ���·� ������
        // �ٱ����� �ʺ�� 1270 ~ 1490, ���̴� -330 ~ 330
        // �ٱ����� �ʺ�� -42 ~ 130, ���̴� -300 ~ 300
        // �ٱ����� �ʺ�� -20 ~ 130, ���̴� -290 ~ 280 
        // �������� 1 ������ ���� 2 �ش������ 0
        
        JuiceInfo savejuiceInfo = null;

        for (int i = 0; i < fruitInfoList.Count; i++)
        {
            if (fruitInfoList[i].juiceName == _juiceName)
            {
                savejuiceInfo = fruitInfoList[i];
            }
        }

        TextuerListCheck(savejuiceInfo.leftCupFruit_Image);
        TextuerListCheck(savejuiceInfo.rightCupFruit_Image);

        GameManager.Inst.Set_MaxScore(savejuiceInfo.leftCupCount + savejuiceInfo.rightCupCount);

        leftCup.GetComponent<FruitCase>().SetCupMaxCount(savejuiceInfo.leftCupCount);
        rightCup.GetComponent<FruitCase>().SetCupMaxCount(savejuiceInfo.rightCupCount);

        //�� ���� Color �� Sprite �ֱ�
        leftCup.GetComponent<FruitCase>().color = getCupColorFromSprite(savejuiceInfo.leftColorCup_Image);
        leftCup.sprite = savejuiceInfo.leftColorCup_Image;
        leftCup.GetComponent<FruitCase>().isRight = false;

        rightCup.GetComponent<FruitCase>().color = getCupColorFromSprite(savejuiceInfo.rightColorCup_Image);
        rightCup.sprite = savejuiceInfo.rightColorCup_Image;
        rightCup.GetComponent<FruitCase>().isRight = true;

        AnswerFruintSeting(savejuiceInfo.leftCupCount, savejuiceInfo.leftCupFruit_Image, 1, true);  // ���� ��
        AnswerFruintSeting(savejuiceInfo.rightCupCount, savejuiceInfo.rightCupFruit_Image, 2, true);  // ������ ��
        AnswerFruintSeting(savejuiceInfo.fruitCount - (savejuiceInfo.leftCupCount + savejuiceInfo.rightCupCount), fruitTextuerList, 0, false);


        // ���ӸŴ����� ����
        SaveTexture(GameManager.Inst.Get_FruitTexterList, saveFruitTextuerList);
        SaveSlice(GameManager.Inst.Get_SliceTexterList, sliceTextuerList, saveFruitTextuerList, "1");
        SaveSlice(GameManager.Inst.Get_SliceTexterList, sliceTextuerList, saveFruitTextuerList, "2");

        Debug.Log("�߰��� �����̽��� ������ : " + GameManager.Inst.Get_SliceTexterList.Count);

        GameManager.Inst.saveJuiceImage = savejuiceInfo.juiceImage;
        GameManager.Inst.saveJuiceName = savejuiceInfo.juiceName;
        Debug.Log("List : " + saveFruitTextuerList.Count);
    }
    //�����Ҽ��������غ���
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

    public void TextuerListCheck(List<Sprite> _selectList)
    {
        for (int i = 0; i < _selectList.Count; i++)
        {
            fruitTextuerList.Remove(_selectList[i]);
        }
    }

    // ������ ��ġ�� ���� ������Ű�� �Լ�
    // ������ ������ ����, 
    public void AnswerFruintSeting(int _fruitNum, List<Sprite> _fruitTexterList, int _id, bool _dirListOn)
    {
        Image fruit = null;
        int posIdx = 0;
        int fruitIdx = 0;
        
        for (int i = 0; i < _fruitNum; i++)
        {
            fruitIdx = Random.Range(0, _fruitTexterList.Count);
            fruit = Instantiate(fruitPrefab);
            fruit.transform.GetChild(0).GetComponent<Image>().sprite = _fruitTexterList[fruitIdx];
            fruit.transform.GetChild(0).GetComponent<FruitID>().id = _id;

            if (_dirListOn)
            {
                Debug.Log(_fruitTexterList[fruitIdx].name);
                saveFruitTextuerList.Add(_fruitTexterList[fruitIdx]);
            }
            _fruitTexterList.Remove(_fruitTexterList[fruitIdx]);

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
