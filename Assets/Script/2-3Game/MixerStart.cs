using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MixerStart : MonoBehaviour
{
    [SerializeField] private GameObject mixerWall = null;           // �߸� ���ϵ��� ������ ����
    [SerializeField] private GameObject rightMove_JuiceObj = null;  // �������� �̵��� �ֽ� �̹���
    [SerializeField] private GameObject upMove_JuiceObj = null;     // ������� �̵��� �ֽ� �̹���
    [SerializeField] private GameObject mixerUi = null;             // �ͼ��� ������ Ui
    [SerializeField] private GameObject mixerBackGround1 = null;    // �ͼ��� ���� ���1
    [SerializeField] private GameObject mixerBackGround2 = null;    // �ͼ��� ���� ���2
    [SerializeField] private GameObject mixerBackGround3 = null;    // �ͼ��� ���� ���3
    [SerializeField] private GameObject mixer = null;               // �ͼ���
    [SerializeField] private GameObject mixerButtonGuide1 = null;   // �ͼ��� ��ư�� ������� ���̵� �հ���
    [SerializeField] private GameObject mixerButtonGuide2 = null;   // �ͼ��� ��ư�� ������� ���̵� �ؽ�Ʈ
    [SerializeField] private Button mixerButton = null;             // �ͼ��� ��ư
    [SerializeField] private GameObject mixerEndObj;                // �ͼ��� ���� ����� �����ϴ� �̹���

    public GameObject movePos = null;                               //

    // �ͼ��� ���� ���۽� Ȱ��ȭ
    public void MixerGameOn()
    {
        mixerWall.SetActive(true);
        rightMove_JuiceObj.SetActive(true);
        upMove_JuiceObj.SetActive(true);
        mixerButton.gameObject.SetActive(true);
    }

    // �ͼ��� ���� ����� ȣ��
    public void MixerGameEnd()
    {
        SoundManager.Inst.StopSFX();
        StartCoroutine(MixerGameAlpha());
    }

    // ��� ������Ʈ�� ���İ��� ����� ���� �̹��� Ȱ��ȭ
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
