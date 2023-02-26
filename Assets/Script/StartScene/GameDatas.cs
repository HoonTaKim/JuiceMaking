using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum DIFFICULTY
{
    EASY,
    NORMAL,
    HARD,
    MASTER
}

[System.Serializable]
public class GameSeting
{
    public float cutGameTime = 0f;      // �ڸ�������� ���� �ð�
    public int reward_Min = 0;          // �ּ� ������
    public int reward_Max = 0;          // �ִ� ������
    public int reward_Terms = 0;        // ������ Max�� �ޱ����� ����
    public List<float> timeSection_Min = new List<float>();         // ������ ������ �ּҽð�
    public List<float> timeSection_Max = new List<float>();         // ������ ������ �ּҽð�

    [Header("���� ����")]
    public List<int> creatFruit_MinCount = new List<int>();         // �� �������� �����Ǵ� �ּҰ���
    public List<int> creatFruit_MaxCount = new List<int>();         // �� �������� �����Ǵ� �ִ밳��
    public float fruitTimeSection_Min = 0f;         // ������ ���� �ּ� �ð�
    public float fruitTimeSection_Max = 0f;         // ������ �ִ� ���� �ð�
    public int fruitScore = 0;          // ������ ����

    [Header("��ź ����")]
    public int boomTimeSection = 0;         // ��ź�� ���� �ֱ�
    public int creatBoomCount = 0;          // ��ź�� ���� ����
    public int boomScore = 0;               // ��ź�� ����
    public int boomTime = 0;                // ��ź���� ���̴� �ð�
    public List<int> boomAddCreatCount = new List<int>();       // ��ź�� �߰� ���� ����
}

public class GameDatas : MonoBehaviour
{
    public static GameDatas Inst;
    public DIFFICULTY difficulty;

    [SerializeField] private List<GameSeting> setingList = new List<GameSeting>();
    private GameSeting selectSeting = null;
    public int score = 0;
    public int reward = 0;

    public GameSeting Get_SelectSeting { get { return selectSeting; } private set { } }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Inst == null) Inst = this;
        else Destroy(gameObject);

        SoundManager.Inst.PlayBGM("TalkingCuteChiptune");
    }

    public void SelectIdx(int idx)
    {
        selectSeting = setingList[idx];
        //selectSeting.boomScore *= -1;
        //selectSeting.boomTime *= -1;

        if (selectSeting.boomTime == -1)
        {
            selectSeting.boomTime = 0;
        }

        SoundManager.Inst.StopBGM();
        SoundManager.Inst.PlayBGM("Hoppin_#039J");

        Debug.Log("������ �ð� : " + selectSeting.cutGameTime);
        Debug.Log("������ �ּ� : " + selectSeting.reward_Min);
        Debug.Log("������ �ִ� : " + selectSeting.reward_Max);
        Debug.Log("������ ���� : " + selectSeting.reward_Terms);
        Debug.Log("�ð� ������ ���� �ּ� ī��Ʈ : " + selectSeting.timeSection_Min.Count);
        Debug.Log("�ð� ������ ���� �ִ� ī��Ʈ : " + selectSeting.timeSection_Max.Count);
        Debug.Log("������ �ּ� �����ð� : " + selectSeting.fruitTimeSection_Min);
        Debug.Log("������ �ִ� �����ð� : " + selectSeting.fruitTimeSection_Max);
        Debug.Log("������ ���� : " + selectSeting.fruitScore);
        Debug.Log("��ź�� ���� �ֱ� : " + selectSeting.boomTimeSection);
        Debug.Log("��ź�� ���� ���� : " + selectSeting.creatBoomCount);
        Debug.Log("��ź�� ���� : " + selectSeting.boomScore);
        Debug.Log("��ź���� ���̴� �ð� : " + selectSeting.boomTime);
        Debug.Log("��ź�� �߰� ���� ���� : " + selectSeting.boomAddCreatCount.Count);
    }

    public void RewardSeting(int score)
    {
        Debug.Log("���� ���ھ� : " + score);

        this.score = score;

        if (score >= selectSeting.reward_Terms)
        {
            reward = selectSeting.reward_Max;
        }
        else
        {
            reward = selectSeting.reward_Min;
        }

        Debug.Log("������ �ּ� : " + selectSeting.reward_Min);
    }
}
