using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI time_Text = null;
    [SerializeField] private TextMeshProUGUI score_Text = null;
    [SerializeField] private TextMeshProUGUI combo_Text = null;
    [SerializeField] private TextMeshProUGUI scoreMinus_Text = null;
    [SerializeField] private TextMeshProUGUI timeMinus_Text = null;
    [SerializeField] private RawImage screenCover = null;
    [SerializeField] private RawImage startCountImage = null;
    [SerializeField] private RawImage startCountEndImage = null;
    [SerializeField] private List<Texture2D> startCountImageList = null;
    [SerializeField] private GameObject mixer = null;
    [SerializeField] private Image gameEndImage = null;

    [SerializeField] private GameObject sliceGuide = null;
    
    private int scoreSave = 0;

    [SerializeField] private Button mixButton = null;
    //[SerializeField] private Slider mixSlider = null;
    [SerializeField] private float[] sliderValue = new float[4];
    [SerializeField] private float[] sliderSecond = new float[3];
    [SerializeField] private GameObject moveJucie = null;
    private bool mixButtonOn = false;
    private float buttonTime = 0f;
    private bool gameSet = false;
    public bool Get_MixButtonOn { get { return mixButtonOn; } private set { } }

    private void Start()
    {
        GameManager.Inst.cutGameStart = true;
    }

    public void StartCount()
    {
        StartCoroutine(StartCountOn());
    }

    private IEnumerator StartCountOn()
    {
        FindObjectOfType<CommonUI>().gameObject.SetActive(true);

        startCountImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        startCountImage.texture = startCountImageList[1];
        yield return new WaitForSeconds(1);
        startCountImage.texture = startCountImageList[2];
        yield return new WaitForSeconds(1);
        startCountImage.gameObject.SetActive(false);
        startCountEndImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        startCountEndImage.gameObject.SetActive(false);

        GameManager.Inst.cutStart = true;

        sliceGuide.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.4f);
        sliceGuide.gameObject.SetActive(false);
    }

    #region CutGame
    public void UiOff()
    {
        time_Text.gameObject.SetActive(false);
        score_Text.gameObject.SetActive(false);
        combo_Text.gameObject.SetActive(false);
        GameEndImageSetActive(false);
    }


    public void MixOn()
    {
        mixer.gameObject.SetActive(true);
    }

    public void TimeText(float _time)
    {
        _time = Mathf.Floor(_time);
        time_Text.text = _time.ToString();
    }

    public void MimusTime(float time, string minus)
    {
        if (minus == "0") return;

        time = Mathf.Floor(time);
        timeMinus_Text.text = minus;
        time_Text.text = "-" + time.ToString();
        StartCoroutine(MinusText(timeMinus_Text));
    }

    public void ScoreText(int _score)
    {
        score_Text.text = "Á¡¼ö : " + _score.ToString();
    }

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

    public void ComboText(int _combo)
    {
        StartCoroutine(Combo(_combo));
    }

    IEnumerator Combo(int _combo)
    {
        combo_Text.gameObject.SetActive(true);
        combo_Text.text = _combo + " ÄÞº¸!";
        yield return new WaitForSeconds(1f);
        combo_Text.gameObject.SetActive(false);
    }

    public void ComboTextOff()
    {
        combo_Text.gameObject.SetActive(false);
    }

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

    public void GameEndImageSetActive(bool _set)
    {
        gameEndImage.gameObject.SetActive(_set);
    }
    #endregion

    #region MixerGame
    private void Update()
    {
        if (mixButtonOn)
        {
            buttonTime += Time.deltaTime;

            //if (buttonTime > sliderSecond[0] && buttonTime < sliderSecond[1])
            //    mixSlider.value += sliderValue[1]; // 0.002f;

            //else if (buttonTime > sliderSecond[1] && buttonTime < sliderSecond[2])
            //    mixSlider.value += sliderValue[2]; // 0.0025f;

            //else if (buttonTime > sliderSecond[2])
            //    mixSlider.value += sliderValue[3]; // 0.0029f;

            //else
            //    mixSlider.value += sliderValue[0]; // 0.0015f;
        }
    }

    public void OnButtonDown()
    {
        if (gameSet) return;
        mixButtonOn = true;
    }

    public void OnButtonUp()
    {
        if (moveJucie.transform.position.y >= -0.25f)
        {
            GameManager.Inst.MixerGameEnd();
        }
        mixButtonOn = false;
        //StartCoroutine(MixerVibrationOff());
        buttonTime = 0f;
    }
    #endregion
}
