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
    [SerializeField] private BoomInfo boom_Obj = null;  // 폭탄 오브젝트
    private FruitInfo fruit = null;                     // 오브젝트 풀로 생성할 오브젝트

    private float randomPos_X = 0f;
    private float upPower = 0f;
    private float sidePower = 0f;
    private bool restOn = false;                        // 휴식 시작

    [Header("SpawnInfo")]
    private int boomNum = 0;
    private int listIdx = 0;

    [Header("SpawnTime")]
    private float fruit_PeriodTime = 0f;   // 과일 생성 기준 시간
    private float boom_PeriodTime = 0f;    // 폭탄 생성 기준 시간
    private float refresh_PeriodTime = 0f;    // 개체 생성 중지의 기준 시간
    private float refresh_Time = 0f;      // 개체 생성 중지의 유지 시간
    private WaitForSeconds waitRefresh;

    [Header("SpawnSaveTime")]
    private float fruit_spawnTime = 0f;     // 과일의 생성 시간 저장
    private float boom_spawnTime = 0f;      // 폭탄의 생성 시간 저장
    private float stop_SpawnTime = 0f;     // 개체 생성 중지를 저장


    private bool refresh = false;                           // 휴식중인지 판단

    private void Start()
    {
        Init();
    }

    // 값 초기화
    private void Init()
    {
        fruit = null;
        fruit_spawnTime = 0f;
        boom_spawnTime = 0f;
        randomPos_X = 0f;
        restOn = false;
        boom_PeriodTime = GameDatas.Inst.boomTimeSection;
        boomNum = GameDatas.Inst.creatBoomCount;

        refresh_PeriodTime = GameDatas.Inst.refreshCycle;
        refresh_Time = GameDatas.Inst.refreshTime;
        waitRefresh = new WaitForSeconds(refresh_Time);
    }

    // 게임이 종료될때까지 Spawn
    private void Update()
    {
        if (GameManager.Inst.Get_GameSet || !GameManager.Inst.Get_CutGameStart) return;

        Spawn();
    }

    // 스폰 시간 저장 및 시간에 따라 각기 다른 오브젝트들  생성
    private void Spawn()
    {
        if (restOn) return;
        SpawnPhase();
        if (refresh) return;

        fruit_spawnTime += Time.deltaTime;
        boom_spawnTime += Time.deltaTime;
        stop_SpawnTime += Time.deltaTime;

        fruit_PeriodTime = Random.Range(GameDatas.Inst.fruitTimeSection_Min, GameDatas.Inst.fruitTimeSection_Max);
        
        if (fruit_spawnTime >= fruit_PeriodTime)
            FruitSpwn(GameDatas.Inst.creatFruit_MinCount[listIdx], GameDatas.Inst.creatFruit_MaxCount[listIdx]);
        if (boom_spawnTime >= boom_PeriodTime)
            BoomSpwn();
        if (stop_SpawnTime >= refresh_PeriodTime)
            StopTime();

        for (int i = 0; i < GameDatas.Inst.boomAddCreatCount.Count; i++)
        {
            if (GameManager.Inst.curTime == GameDatas.Inst.boomAddCreatCount[i])
            {
                BoomSpwn();
            }
        }
    }

    // 페이즈에 따라 생성개수가 달라지기 때문에 나눈 페이즈
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
    

    // 과일 생성
    private void FruitSpwn(int _min, int _max)
    {
        for (int i = 0; i < Random.Range(_min, _max); i++)
        {
            randomPos_X = Random.Range(-9f, 9f);
            upPower = Random.Range(580f, 620f);
            fruit = FruitPooling.Inst.Out(randomPos_X, -6f);

            // 포지션이 음수일때는 오른쪽으로 힘을가함
            // 포지션이 음수일때는 회전을 오른쪽으로
            // 포지션이 -9와 가까울수록 오른쪽으로 가하는힘이 강해짐
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

            // 포지션이 양수일때는 왼쪽으로 힘을 가함
            // 포지션이 양수일때는 회전을 왼쪽으로
            // 포지션이 9와 가까울수록 왼쪽으로 가하는힘이 강해짐
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

    // 폭탄 생성
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

            // 포지션이 음수일때는 오른쪽으로 힘을가함
            // 포지션이 음수일때는 회전을 오른쪽으로
            // 포지션이 -9와 가까울수록 오른쪽으로 가하는힘이 강해짐
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

            // 포지션이 양수일때는 왼쪽으로 힘을 가함
            // 포지션이 양수일때는 회전을 왼쪽으로
            // 포지션이 9와 가까울수록 왼쪽으로 가하는힘이 강해짐
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

    // 휴식 시작
    private void StopTime()
    {
        if (!refresh)
        {
            StartCoroutine(RefreshTime());
        }
    }

    IEnumerator RefreshTime()
    {
        refresh = true;
        yield return waitRefresh;
        refresh = false;

        stop_SpawnTime = 0f;
    }
}
