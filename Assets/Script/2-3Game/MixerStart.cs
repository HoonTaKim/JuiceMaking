using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MixerStart : MonoBehaviour
{
    [SerializeField] private GameObject mixerWall = null;           // 잘린 과일들이 위차할 공간
    [SerializeField] private GameObject rightMove_JuiceObj = null;  // 우측으로 이동할 주스 이미지
    [SerializeField] private GameObject upMove_JuiceObj = null;     // 상단으로 이동할 주스 이미지
    [SerializeField] private GameObject mixerUi = null;             // 믹서기 게임의 Ui
    [SerializeField] private GameObject mixerBackGround1 = null;    // 믹서기 게임 배경1
    [SerializeField] private GameObject mixerBackGround2 = null;    // 믹서기 게임 배경2
    [SerializeField] private GameObject mixerBackGround3 = null;    // 믹서기 게임 배경3
    [SerializeField] private GameObject mixer = null;               // 믹서기
    [SerializeField] private GameObject mixerButtonGuide1 = null;   // 믹서기 버튼을 누르라는 가이드 손가락
    [SerializeField] private GameObject mixerButtonGuide2 = null;   // 믹서기 버튼을 누르라는 가이드 텍스트
    [SerializeField] private Button mixerButton = null;             // 믹서기 버튼
    [SerializeField] private GameObject mixerEndObj;                // 믹서기 게임 종료시 등장하는 이미지

    public GameObject movePos = null;                               //

    // 믹서기 게임 시작시 활성화
    public void MixerGameOn()
    {
        mixerWall.SetActive(true);
        rightMove_JuiceObj.SetActive(true);
        upMove_JuiceObj.SetActive(true);
        mixerButton.gameObject.SetActive(true);
    }

    // 믹서기 게임 종료시 호출
    public void MixerGameEnd()
    {
        SoundManager.Inst.StopSFX();
        StartCoroutine(MixerGameAlpha());
    }

    // 모든 오브젝트의 알파값을 낮춘뒤 종료 이미지 활성화
    IEnumerator MixerGameAlpha()
    {
        mixerEndObj.SetActive(true);

        while (mixer.GetComponent<SpriteRenderer>().color.r > 0.5f)
        {
            AlphaDown(mixerBackGround1);
            AlphaDown(mixerBackGround2);
            AlphaDown(mixerBackGround3);
            AlphaDown(mixer);
            AlphaDown(upMove_JuiceObj);
            AlphaDown(mixerButtonGuide1);
            AlphaDown(mixerButtonGuide2);

            yield return new WaitForSeconds(0.001f);
        }

        yield return new WaitForSeconds(3f);
        mixerEndObj.SetActive(false);
        mixerUi.gameObject.SetActive(false);
        SceneManager.LoadScene("Additive_EndScene", LoadSceneMode.Additive);
    }

    public void AlphaDown(GameObject obj)
    {
        obj.GetComponent<SpriteRenderer>().color = new Color(obj.GetComponent<SpriteRenderer>().color.r - 0.01f, obj.GetComponent<SpriteRenderer>().color.g - 0.01f, obj.GetComponent<SpriteRenderer>().color.b - 0.01f);
    }
}
