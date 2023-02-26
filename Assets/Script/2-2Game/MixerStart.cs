using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MixerStart : MonoBehaviour
{
    [SerializeField] private GameObject mixerWall = null;
    [SerializeField] private GameObject mixerObjs = null;
    [SerializeField] private GameObject mixerImage = null;
    [SerializeField] private GameObject mixerUi = null;
    [SerializeField] private GameObject mixerBackGround1 = null;
    [SerializeField] private GameObject mixerBackGround2 = null;
    [SerializeField] private GameObject mixerBackGround3 = null;
    [SerializeField] private GameObject mixer = null;
    [SerializeField] private GameObject juice = null;
    [SerializeField] private GameObject mixerButtonGuide1 = null;
    [SerializeField] private GameObject mixerButtonGuide2 = null;

    [SerializeField] private Button mixerButton = null;

    [SerializeField] private GameObject mixerEndObj;

    public GameObject movePos = null;

    public void MixerGameOn()
    {
        mixerWall.SetActive(true);
        mixerObjs.SetActive(true);
        mixerImage.SetActive(true);
        mixerButton.gameObject.SetActive(true);
    }

    public void MixerGameEnd()
    {
        StartCoroutine(MixerGameAlpha());
    }

    IEnumerator MixerGameAlpha()
    {
        mixerEndObj.SetActive(true);

        while (mixer.GetComponent<SpriteRenderer>().color.r > 0.5f)
        {
            AlphaDown(mixerBackGround1);
            AlphaDown(mixerBackGround2);
            AlphaDown(mixerBackGround3);
            AlphaDown(mixer);
            AlphaDown(juice);
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
