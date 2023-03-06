using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI time_Text = null;              // ���� ���� ǥ�� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI score_Text = null;             // ���� ǥ�� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI combo_Text = null;             // �޺� ǥ�� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI scoreMinus_Text = null;        // ���� ���� �ؽ�Ʈ
    [SerializeField] private TextMeshProUGUI timeMinus_Text = null;         // �ð� ���� �ؽ�Ʈ
    [SerializeField] private RawImage screenCover = null;                   // ������ ���� Ŀ��
    [SerializeField] private RawImage startCountImage = null;
    [SerializeField] private RawImage startCountEndImage = null;            // 
    [SerializeField] private List<Texture2D> startCountImageList = null;    // �ڸ��� ���� ���� ī��Ʈ �̹��� ����Ʈ
    [SerializeField] private GameObject mixer = null;                       // ��� �ͼ��� ���� ������Ʈ
    [SerializeField] private Image gameEndImage = null;                     // ���� ���� �̹���
    [SerializeField] private GameObject sliceGuide = null;                  // �����̵� ���̵� ������Ʈ
    
    [SerializeField] private GameObject moveJucie = null;                   // �ͼ��� ������ �����̴� �ֽ� ������Ʈ
    private bool mixButtonOn = false;                                       // �ͼ����� ��ư�� ���ȴ��� Ȯ���ϴ� bool��
    private bool mixerGameSet = false;                                      // �ͼ��� ������ ���� Ȯ�� bool��
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

    // �ڸ��� ���� ���� ���� �ڷ�ƾ
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

    // �ڸ��� ���� ����� UI��Ȱ��ȭ ��Ű�� �Լ�
    public void UiOff()
    {
        time_Text.gameObject.SetActive(false);
        score_Text.gameObject.SetActive(false);
        combo_Text.gameObject.SetActive(false);
        GameEndImageSetActive(false);
    }

    // ���� �ð� �ؽ�Ʈ�� �ֽ�ȭ
    public void TimeText(float _time)
    {
        _time = Mathf.Floor(_time);
        time_Text.text = _time.ToString();
    }

    // ���ھ� �ؽ�Ʈ �ֽ�ȭ
    public void ScoreText(int _score)
    {
        score_Text.text = "���� : " + _score.ToString();
    }

    // ���� ������ �ڸ��� �ð�, ������ �ؽ�Ʈ�� �ֽ�ȭ
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

    // ���̳ʽ��Ǵ� �ؽ�Ʈ�� ��Ȱ��ȭ
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

    // �޺� �ؽ�Ʈ Ȱ��ȭ
    public void ComboText(int _combo)
    {
        StartCoroutine(Combo(_combo));
    }

    IEnumerator Combo(int _combo)
    {
        combo_Text.gameObject.SetActive(true);
        combo_Text.text = _combo + " �޺�!";
        yield return new WaitForSeconds(1f);
        combo_Text.gameObject.SetActive(false);
    }

    // �޺��ؽ�Ʈ ��Ȱ��ȭ
    public void ComboTextOff()
    {
        combo_Text.gameObject.SetActive(false);
    }

    // ���� ���۽� ������ Ŀ���� �̵�
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

    // �ڸ��� ���� ����� ���� �̹��� Ȱ��ȭ, ��Ȱ��ȭ ��Ű�� �Լ�
    public void GameEndImageSetActive(bool _set)
    {
        gameEndImage.gameObject.SetActive(_set);
    }
    #endregion

    #region MixerGame

    // �ͼ��� ���� ���۽� ��� ������Ʈ Ȱ��ȭ
    public void MixOn()
    {
        mixer.gameObject.SetActive(true);
    }

    // �ͼ��� ��ư�� �������� ����
    public void OnButtonDown()
    {
        if (mixerGameSet) return;
        mixButtonOn = true;
    }

    // �ͼ��� ��ư���� ���� ������ ����
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
