using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TimeSeting
{
    public float time_Min = 0f;
    public float time_Max = 0f;
}

[System.Serializable]
public class FruitNum
{
    public int fruit_Min = 0;
    public int fruit_Max = 0;
}

public class FruitSpwner : MonoBehaviour
{
    [SerializeField] private BoomInfo boom_Obj = null;
    private FruitInfo fruit = null;


    private float randomPos_X = 0f;
    private float upPower = 0f;
    private float sidePower = 0f;
    private bool restOn = false;

    [SerializeField] private float testPower = 0f;
    [SerializeField] private float testRandomPos_X = 0f;
    [SerializeField] private float testPushPower = 0f;


    [Header("SpawnInfo")]
    private int boomNum = 0;
    private int listIdx = 0;

    [Header("SpawnTime")]
    private float time = 0f;
    private float fruit_PeriodTime = 0f;   // ���� ���� ���� �ð�
    private float boom_PeriodTime = 0f;    // ��ź ���� ���� �ð�
    [SerializeField] private float stop_PeriodTime = 0f;    // ��ü ���� ������ ���� �ð�
    [SerializeField] private float stop_KeepTime = 0f;      // ��ü ���� ������ ���� �ð�

    [Header("SpawnSaveTime")]
    private float fruit_spawnTime = 0f;     // ������ ���� �ð� ����
    private float boom_spawnTime = 0f;      // ��ź�� ���� �ð� ����
    private float stop_SpawnTime = 0f;     // ��ü ���� ������ ����


    private bool refresh = false;                           // �޽������� �Ǵ�

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        fruit = null;
        fruit_spawnTime = 0f;
        boom_spawnTime = 0f;
        randomPos_X = 0f;
        restOn = false;
        boom_PeriodTime = GameDatas.Inst.boomTimeSection;
        boomNum = GameDatas.Inst.creatBoomCount;
    }

    private void Update()
    {
        if (GameManager.Inst.Get_GameSet || !GameManager.Inst.Get_CutGameStart) return;

        Spawn();
    }

    private void Spawn()
    {
        if (restOn) return;
        SpawnPhase();
        if (refresh) return;

        time += Time.deltaTime;
        fruit_spawnTime += Time.deltaTime;
        boom_spawnTime += Time.deltaTime;
        stop_SpawnTime += Time.deltaTime;

        fruit_PeriodTime = Random.Range(GameDatas.Inst.fruitTimeSection_Min, GameDatas.Inst.fruitTimeSection_Max);
        
        if (fruit_spawnTime >= fruit_PeriodTime)
            FruitSpwn(GameDatas.Inst.creatFruit_MinCount[listIdx], GameDatas.Inst.creatFruit_MaxCount[listIdx]);
        if (boom_spawnTime >= boom_PeriodTime)
            BoomSpwn();
        if (stop_PeriodTime >= stop_SpawnTime)
            StopTime();

        for (int i = 0; i < GameDatas.Inst.boomAddCreatCount.Count; i++)
        {
            if (GameManager.Inst.curTime == GameDatas.Inst.boomAddCreatCount[i])
            {
                BoomSpwn();
            }
        }
    }

    private void SpawnPhase()
    {
        switch (GameManager.Inst.Get_PAGE)
        {
            case GameManager.PAGE.PAGE1:
                listIdx = 0;
                break;
            case GameManager.PAGE.PAGE2:
                listIdx = 1;
                break;
            case GameManager.PAGE.PAGE3:
                listIdx = 2;
                break;
            case GameManager.PAGE.PAGE4:
                listIdx = 3;
                break;
        }
    }
    

    // �ϵ��ڵ� ����
    private void FruitSpwn(int _min, int _max)
    {
        for (int i = 0; i < Random.Range(_min, _max); i++)
        {
            randomPos_X = Random.Range(-9f, 9f);
            upPower = Random.Range(580f, 620f);
            fruit = FruitPooling.Inst.Out(randomPos_X, -6f);

            // �������� �����϶��� ���������� ��������
            // �������� �����϶��� ȸ���� ����������
            // �������� -9�� �������� ���������� ���ϴ����� ������
            if (randomPos_X < 0)
            {
                if (randomPos_X >= -9 || randomPos_X <= -6)
                {
                    sidePower = Random.Range(140f, 325f);
                }
                if (randomPos_X >= -5 || randomPos_X <= -4)
                {
                    sidePower = Random.Range(120f, 290f);
                }
                if (randomPos_X >= -3 || randomPos_X <= -1)
                {
                    sidePower = Random.Range(60f, 200f);
                }
                fruit.GetComponent<Rigidbody2D>().AddForce(Vector2.up * upPower);
                fruit.GetComponent<Rigidbody2D>().AddForce(Vector2.right * sidePower);
                fruit.GetComponent<Rigidbody2D>().AddTorque(Random.Range(30f, 70f));
            }

            // �������� ����϶��� �������� ���� ����
            // �������� ����϶��� ȸ���� ��������
            // �������� 9�� �������� �������� ���ϴ����� ������
            if (randomPos_X > 0)
            {
                if (randomPos_X >= 6 || randomPos_X <= 9)
                {
                    sidePower = Random.Range(140f, 325f);
                }
                if (randomPos_X >= 4|| randomPos_X <= 5)
                {
                    sidePower = Random.Range(120f, 290f);
                }
                if (randomPos_X >= 1 || randomPos_X <= 3)
                {
                    sidePower = Random.Range(60f, 200f);
                }
                fruit.GetComponent<Rigidbody2D>().AddForce(Vector2.up * upPower);
                fruit.GetComponent<Rigidbody2D>().AddForce(Vector2.left * sidePower);
                fruit.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-70f, -30));
            }
        }
        fruit_spawnTime = 0f;
    }

    private void BoomSpwn()
    {
        float randomPos_X = 0f;
        for (int i = 0; i < boomNum; i++)
        {
            randomPos_X = Random.Range(-8f, 8f);
            BoomInfo boom = Instantiate(boom_Obj);
            boom.transform.position = new Vector3(randomPos_X, -6f, -1);
            boom.transform.rotation = Quaternion.identity;
            boom.gameObject.SetActive(true);

            // �������� �����϶��� ���������� ��������
            // �������� �����϶��� ȸ���� ����������
            // �������� -9�� �������� ���������� ���ϴ����� ������
            if (randomPos_X < 0)
            {
                if (randomPos_X >= -9 || randomPos_X <= -6)
                {
                    sidePower = Random.Range(140f, 325f);
                }
                if (randomPos_X >= -5 || randomPos_X <= -4)
                {
                    sidePower = Random.Range(120f, 290f);
                }
                if (randomPos_X >= -3 || randomPos_X <= -1)
                {
                    sidePower = Random.Range(60f, 200f);
                }
                boom.GetComponent<Rigidbody2D>().AddForce(Vector2.up * upPower);
                boom.GetComponent<Rigidbody2D>().AddForce(Vector2.right * sidePower);
                boom.GetComponent<Rigidbody2D>().AddTorque(Random.Range(30f, 70f));
            }

            // �������� ����϶��� �������� ���� ����
            // �������� ����϶��� ȸ���� ��������
            // �������� 9�� �������� �������� ���ϴ����� ������
            if (randomPos_X > 0)
            {
                if (randomPos_X >= 6 || randomPos_X <= 9)
                {
                    sidePower = Random.Range(140f, 325f);
                }
                if (randomPos_X >= 4 || randomPos_X <= 5)
                {
                    sidePower = Random.Range(120f, 290f);
                }
                if (randomPos_X >= 1 || randomPos_X <= 3)
                {
                    sidePower = Random.Range(60f, 200f);
                }
                boom.GetComponent<Rigidbody2D>().AddForce(Vector2.up * upPower);
                boom.GetComponent<Rigidbody2D>().AddForce(Vector2.left * sidePower);
                boom.GetComponent<Rigidbody2D>().AddTorque(Random.Range(-70f, -30));
            }
            //boom.GetComponent<Rigidbody2D>().AddForce(Vector2.up * upPower);
            //boom.GetComponent<Rigidbody2D>().AddTorque(Random.Range(60f, 70f));
        }

        boom_spawnTime = 0;
    }

    private void StopTime()
    {
        //refresh = true;

    }
}
