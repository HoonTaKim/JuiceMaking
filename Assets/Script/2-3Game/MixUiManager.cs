using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MixUiManager : MonoBehaviour
{
    [SerializeField] private Button mixButton = null;
    [SerializeField] private Slider mixSlider = null;
    [SerializeField] private TextMeshProUGUI tmp = null;
    [SerializeField] private TextMeshProUGUI gameEndText = null;
    [SerializeField] private RawImage mixerImage = null;

    [SerializeField] private float[] sliderValue = new float[4];
    [SerializeField] private float[] sliderSecond = new float[3];

    private bool mixButtonOn = false;
    private float buttonTime = 0f;
    private bool gameSet = false;

    public bool Get_MixButtonOn { get { return mixButtonOn; } private set { } }

    private void Awake()
    {
        mixSlider.value = 0;
    }

    // 게임매니저 업데이트로 뺄것
    private void Update()
    {
        if (mixButtonOn)
        {
            buttonTime += Time.deltaTime;
            tmp.text = buttonTime.ToString("F1");

            if (buttonTime > sliderSecond[0] && buttonTime < sliderSecond[1])
                mixSlider.value += sliderValue[1]; // 0.002f;

            else if (buttonTime > sliderSecond[1] && buttonTime < sliderSecond[2])
                mixSlider.value += sliderValue[2]; // 0.0025f;

            else if (buttonTime > sliderSecond[2])
                mixSlider.value += sliderValue[3]; // 0.0029f;

            else 
                mixSlider.value += sliderValue[0]; // 0.0015f;
        }
    }

    public void OnButtonDown()
    {
        if (gameSet) return;
        mixButtonOn = true;
        StartCoroutine(MixerVibrationOn());
    }

    public void OnButtonUp() 
    {
        if (mixSlider.value >= mixSlider.maxValue) 
        {
            gameEndText.text = "GameEnd";
            gameSet = true;
        }
        mixButtonOn = false;
        StartCoroutine(MixerVibrationOff());
        buttonTime = 0f;
    }

    IEnumerator MixerVibrationOn()
    {
        while (mixButtonOn)
        {
            mixerImage.rectTransform.localPosition = new Vector2(mixerImage.rectTransform.localPosition.x + 10f, mixerImage.rectTransform.localPosition.y);
            yield return new WaitForSeconds(0.01f);
            mixerImage.rectTransform.localPosition = new Vector2(mixerImage.rectTransform.localPosition.x - 10f, mixerImage.rectTransform.localPosition.y);
        }
    }

    IEnumerator MixerVibrationOff()
    {
        SoundManager.Inst.StopSFX();

        while (mixerImage.rectTransform.localPosition.x == 0)
        {
            if (mixerImage.rectTransform.localPosition.x > 0)
            {
                mixerImage.rectTransform.localPosition = new Vector2(mixerImage.rectTransform.localPosition.x - 0.01f, mixerImage.rectTransform.localPosition.y);
                yield return new WaitForSeconds(0.01f);
            }
            if (mixerImage.rectTransform.localPosition.x < 0)
            {
                mixerImage.rectTransform.localPosition = new Vector2(mixerImage.rectTransform.localPosition.x + 0.01f, mixerImage.rectTransform.localPosition.y);
                yield return new WaitForSeconds(0.01f);
            }
        }
    }
}
