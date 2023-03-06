using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI time_Text = null;              // 게임 시작 표시 텍스트
    [SerializeField] private TextMeshProUGUI score_Text = null;             // 점수 표시 텍스트
    [SerializeField] private TextMeshProUGUI combo_Text = null;             // 콤보 표시 텍스트
    [SerializeField] private TextMeshProUGUI scoreMinus_Text = null;        // 점수 감소 텍스트
    [SerializeField] private TextMeshProUGUI timeMinus_Text = null;         // 시간 감소 텍스트
    [SerializeField] private RawImage screenCover = null;                   // 검은색 게임 커버
    [SerializeField] private RawImage startCountImage = null;
    [SerializeField] private RawImage startCountEndImage = null;            // 
    [SerializeField] private List<Texture2D> startCountImageList = null;    // 자르기 게임 시작 카운트 이미지 리스트
    [SerializeField] private GameObject mixer = null;                       // 모든 믹서기 게임 오브젝트
    [SerializeField] private Image gameEndImage = null;                     // 게임 종료 이미지
    [SerializeField] private GameObject sliceGuide = null;                  // 슬라이드 가이드 오브젝트
    
    [SerializeField] private GameObject moveJucie = null;                   // 믹서기 내부의 움직이는 주스 오브젝트
    private bool mixButtonOn = false;                                       // 믹서기의 버튼이 눌렸는지 확인하는 bool값
    private bool mixerGameSet = false;                                      // 믹서기 게임의 종료 확인 bool값
    public bool Get_MixButtonOn { get { return mixButtonOn; } private set { } }

    private void Start()
    {
        GameManager.Inst.cutGameStart = true;
    }


    #region CutGame
    public void StartCount()
    {
        StartCoroutine(cutGameStartCountOn());
    }

    // 자르기 게임 시작 연출 코루틴
    private WaitForSeconds wait1 = new WaitForSeconds(1f);
    private IEnumerator cutGameStartCountOn()
    {
        FindObjectOfType<CommonUI>().gameObject.SetActive(true);

        startCountImage.gameObject.SetActive(true);
        yield return wait1;
        startCountImage.texture = startCountImageList[1];
        yield return wait1;
        startCountImage.texture = startCountImageList[2];
        yield return wait1;
        startCountImage.gameObject.SetActive(false);
        startCountEndImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        startCountEndImage.gameObject.SetActive(false);

        GameManager.Inst.cutStart = true;

        sliceGuide.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.4f);
        sliceGuide.gameObject.SetActive(false);
    }

    // 자르기 게임 종료시 UI비활성화 시키는 함수
    public void UiOff()
    {
        time_Text.gameObject.SetActive(false);
        score_Text.gameObject.SetActive(false);
        combo_Text.gameObject.SetActive(false);
        GameEndImageSetActive(false);
    }

    // 게임 시간 텍스트를 최신화
    public void TimeText(float _time)
    {
        _time = Mathf.Floor(_time);
        time_Text.text = _time.ToString();
    }

    // 스코어 텍스트 최신화
    public void ScoreText(int _score)
    {
        score_Text.text = "점수 : " + _score.ToString();
    }

    // 썩은 과일을 자를시 시간, 점수의 텍스트를 최신화
    public void MinusTime(int boomScore, float boomTime)
    {
        if (boomScore > 0)
        {
            scoreMinus_Text.gameObject.SetActive(true);
            scoreMinus_Text.text = "-" + boomScore.ToString();
            StartCoroutine(MinusText(scoreMinus_Text));
        }
        if (boomTime > 0)
        {
            timeMinus_Text.gameObject.SetActive(true);
            timeMinus_Text.text = "-" + boomTime.ToString();
            StartCoroutine(MinusText(timeMinus_Text));
        }
    }

    // 마이너스되는 텍스트를 비활성화
    IEnumerator MinusText(TextMeshProUGUI _text)
    {
        _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, 1);

        while (_text.color.a > 0)
        {
            _text.color = new Color(_text.color.r, _text.color.g, _text.color.b, _text.color.a - 0.01f);
            yield return new WaitForSeconds(0.01f);
        }

        _text.gameObject.SetActive(false);
    }

    // 콤보 텍스트 활성화
    public void ComboText(int _combo)
    {
        StartCoroutine(Combo(_combo));
    }

    IEnumerator Combo(int _combo)
    {
        combo_Text.gameObject.SetActive(true);
        combo_Text.text = _combo + " 콤보!";
        yield return new WaitForSeconds(1f);
        combo_Text.gameObject.SetActive(false);
    }

    // 콤보텍스트 비활성화
    public void ComboTextOff()
    {
        combo_Text.gameObject.SetActive(false);
    }

    // 게임 시작시 검은색 커버를 이동
    public void ScreenCoverOn()
    {
        RectTransform rt = screenCover.GetComponent<RectTransform>();

        Vector3 pos = new Vector3(0, -2250, 0);
        Move_Time(pos, 1f, rt);
    }

    public void Move_Time(Vector3 destination, float time, RectTransform rt)
    {
        float speed = Vector3.Distance(destination, rt.localPosition) / time;
        StartCoroutine(CO_Move(destination, speed, rt));
    }

    public IEnumerator CO_Move(Vector3 destination, float speed, RectTransform rt)
    {
        while (rt.localPosition != destination)
        {
            rt.anchoredPosition = Vector2.MoveTowards(rt.localPosition, destination, speed * Time.deltaTime);
            yield return null;
        }
    }

    // 자르기 게임 종료시 종료 이미지 활성화, 비활성화 시키는 함수
    public void GameEndImageSetActive(bool _set)
    {
        gameEndImage.gameObject.SetActive(_set);
    }
    #endregion

    #region MixerGame

    // 믹서기 게임 시작시 모든 오브젝트 활성화
    public void MixOn()
    {
        mixer.gameObject.SetActive(true);
    }

    // 믹서기 버튼이 눌렸을때 실행
    public void OnButtonDown()
    {
        if (mixerGameSet) return;
        mixButtonOn = true;
    }

    // 믹서기 버튼에서 손을 땟을때 실행
    public void OnButtonUp()
    {
        if (moveJucie.transform.position.y >= -0.25f)
        {
            GameManager.Inst.MixerGameEnd();
        }
        mixButtonOn = false;
    }
    #endregion
}
