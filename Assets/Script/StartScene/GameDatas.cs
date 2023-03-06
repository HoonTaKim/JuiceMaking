using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DIFFICULTY
{
    EASY,
    NORMAL,
    HARD,
    MASTER
}

public class GameDatas : MonoBehaviour
{
    public static GameDatas Inst;
    public DIFFICULTY difficulty;

    public int score = 0;
    public int reward = 0;
    public TextMeshProUGUI test;

    [Header("���� ����")]
    public float cutGameTime = 0f;                                  // �ڸ�������� ���� �ð�
    public int reward_Min = 0;                                      // �ּ� ������
    public int reward_Max = 0;                                      // �ִ� ������
    public int reward_Terms = 0;                                    // ������ Max�� �ޱ����� ����
    public List<float> timeSection_Min = new List<float>();         // ������ ������ �ּҽð�
    public List<float> timeSection_Max = new List<float>();         // ������ ������ �ּҽð�

    [Header("���� ����")]
    public List<int> creatFruit_MinCount = new List<int>();         // �� �������� �����Ǵ� �ּҰ���
    public List<int> creatFruit_MaxCount = new List<int>();         // �� �������� �����Ǵ� �ִ밳��
    public float fruitTimeSection_Min = 0f;                         // ������ ���� �ּ� �ð�
    public float fruitTimeSection_Max = 0f;                         // ������ �ִ� ���� �ð�
    public int fruitScore = 0;                                      // ������ ����

    [Header("��ź ����")]
    public int boomTimeSection = 0;                                 // ��ź�� ���� �ֱ�
    public int creatBoomCount = 0;                                  // ��ź�� ���� ����
    public int boomScore = 0;                                       // ��ź�� ����
    public int boomTime = 0;                                        // ��ź���� ���̴� �ð�
    public List<int> boomAddCreatCount = new List<int>();           // ��ź�� �߰� ���� ����

    [Header("�޽Ľð� ����")]
    public float refreshTime = 0f;                                  // �޽Ľð�
    public float refreshCycle = 0f;                                 // �޽Ľð� �ֱ�

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (Inst == null) Inst = this;
        else Destroy(gameObject);

        SoundManager.Inst.PlayBGM("TalkingCuteChiptune");
    }

    // ������ �Է�
    public void DataSeting(List<string> dataList, int idx)
    {
        string[] splitData = dataList[idx].Split(",");

        cutGameTime = float.Parse(splitData[2]);
        reward_Min = int.Parse(splitData[3]);
        reward_Max = int.Parse(splitData[4]);
        reward_Terms = int.Parse(splitData[5]);

        timeSection_Min.Add(float.Parse(splitData[6]));
        timeSection_Min.Add(float.Parse(splitData[8]));
        timeSection_Min.Add(float.Parse(splitData[10]));
        timeSection_Min.Add(float.Parse(splitData[12]));

        timeSection_Max.Add(float.Parse(splitData[7]));
        timeSection_Max.Add(float.Parse(splitData[9]));
        timeSection_Max.Add(float.Parse(splitData[11]));
        timeSection_Max.Add(float.Parse(splitData[13]));

        creatFruit_MinCount.Add(int.Parse(splitData[14]));
        creatFruit_MinCount.Add(int.Parse(splitData[16]));
        creatFruit_MinCount.Add(int.Parse(splitData[18]));
        creatFruit_MinCount.Add(int.Parse(splitData[20]));

        creatFruit_MaxCount.Add(int.Parse(splitData[15]));
        creatFruit_MaxCount.Add(int.Parse(splitData[17]));
        creatFruit_MaxCount.Add(int.Parse(splitData[19]));
        creatFruit_MaxCount.Add(int.Parse(splitData[21]));

        fruitTimeSection_Min = float.Parse(splitData[22]);
        fruitTimeSection_Max = float.Parse(splitData[23]);
        fruitScore = int.Parse(splitData[24]);

        boomTimeSection = int.Parse(splitData[25]);
        creatBoomCount = int.Parse(splitData[26]);
        boomScore = int.Parse(splitData[27]);
        boomTime = int.Parse(splitData[28]);

        boomAddCreatCount.Add(int.Parse(splitData[29]));
        boomAddCreatCount.Add(int.Parse(splitData[30]));
        boomAddCreatCount.Add(int.Parse(splitData[31]));
        boomAddCreatCount.Add(int.Parse(splitData[32]));
        boomAddCreatCount.Add(int.Parse(splitData[33]));


        //---------------------------------------------------------------------------------------
        SoundManager.Inst.StopBGM();
        SoundManager.Inst.PlayBGM("Hoppin_#039J");
    }

    // �÷ο�2�� ���� ���� ���� �ð� �Է�
    public void RefreshTimeData(List<string> dataList)
    {
        string[] data = dataList[0].Split(",");

        refreshCycle = float.Parse(data[2]);
        refreshTime = float.Parse(data[3]);
    }

    // ���� ���� ���ھ� ����
    public void RewardSeting(int score)
    {
        Debug.Log("���� ���ھ� : " + score);

        this.score = score;

        if (score >= reward_Terms)
        {
            reward = reward_Max;
        }
        else
        {
            reward = reward_Min;
        }
    }
}
