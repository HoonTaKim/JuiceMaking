using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadSceneManager : MonoBehaviour
{
    static string nextSceneName;
    AsyncOperation operation;


    public Image touchImage;

    public Sprite[] interactSprite;

    int interactSpriteIdx;

    private void Start()
    {
        SoundManager.Inst.PlaySFX("SFX_ChangeScene");
        interactSpriteIdx = Random.Range(0, interactSprite.Length);
        touchImage.sprite = interactSprite[interactSpriteIdx];

        StartCoroutine(CO_LoadSceneAsync());
    }


    public static void LoadSceneAsync(string sceneName)
    {
        nextSceneName = sceneName;
        SceneManager.LoadScene("11. LoadScene");

    }
    IEnumerator CO_LoadSceneAsync()
    {
        Time.timeScale = 1;

        operation = SceneManager.LoadSceneAsync(nextSceneName);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
    public void Btn_Loading()
    {
        Debug.Log("CHANGE");

        interactSpriteIdx += Random.Range(1, interactSprite.Length - 2);
        interactSpriteIdx %= interactSprite.Length;

        touchImage.sprite = interactSprite[interactSpriteIdx];
    }
}
