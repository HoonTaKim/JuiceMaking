using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CommonUI : MonoBehaviour
{
    //MinigameTown�� PackageName
    public string TownPackageName = "com.DefaultCompany.MinigameTownTest";


    public GameObject UIPanel;

    bool SFXOn = true;
    bool BGMOn = true;

    [SerializeField] Sprite[] TogleImages;
    [SerializeField] Image Image_BGM;
    [SerializeField] Image Image_SFX;


    public static CommonUI Inst;


    public GameObject Group_Setting;
    public GameObject Group_Town;

    private void Awake()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += changeBtnStatus;
        }
        else
        {
            Destroy(gameObject);
        }

    }


    /// <summary>
    /// �ش� �Լ� switch������ Scene �̸� �־��ֽø� �˴ϴ�.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    public void changeBtnStatus(Scene scene, LoadSceneMode mode)
    {
        switch(scene.name)
        {
            //UI�� �ʿ���� �� (���̵� ���� �� ��)
            case "01. StartScene":
            case "Additive_EndScene":
            case "11. LoadScene":
                Group_Town.SetActive(false);
                Group_Setting.SetActive(false);
                break;

            //������ ���� ��ư �ʿ��� ��(Ÿ�� ���� �� ��)
            case "1.StartSCene":
                Group_Town.SetActive(true);
                Group_Setting.SetActive(false);
                break;
            //�˾� ��ư �ʿ��� ��(���� ���� �� ���� ��� ��)
            default:
                Group_Town.SetActive(false);
                Group_Setting.SetActive(true);
                break;
                
        }
    }
    public void Btn_Setting()
    {
        //�˾� ����
        UIPanel.SetActive(true);
        Time.timeScale = 0f;
        SoundManager.Inst.PlaySFX("SFX_ClickBtn");
    }

    public void Btn_Restart()
    {
        //restart ��ư �������� ������ ���⿡��
        Time.timeScale = 1f;
        UIPanel.SetActive(false);
        SceneManager.LoadScene("2-1.GameScene");
        Destroy(FindObjectOfType<GameManager>().gameObject);
    }

    public void Btn_Exit()
    {
        GameManager gm = FindObjectOfType<GameManager>();
        Destroy(gm.gameObject);
        //Exit��ư �������� ������ ���⿡��
        Time.timeScale = 1f;
        UIPanel.SetActive(false);
        SoundManager.Inst.PlayBGM("SFX_ChangeScene");
        // ���Ŵ����� ������ ���°�
        SceneManager.LoadScene("1.StartScene");



        SoundManager.Inst.PlaySFX("SFX_ClickBtn");
    }

    public void Btn_Help()
    {
        Debug.Log("�̱���");
        SoundManager.Inst.PlaySFX("SFX_ClickBtn");
    }

    public void Btn_BGM()
    {
        if (BGMOn)
        {
            SoundManager.Inst.setBGMVolume(0);
            BGMOn = false;
            Image_BGM.sprite = TogleImages[1];
        }
        else
        {
            SoundManager.Inst.setBGMVolume(1);
            BGMOn = true;
            Image_BGM.sprite = TogleImages[0];
        }
        SoundManager.Inst.PlaySFX("SFX_ClickBtn");
    }

    public void Btn_SFX()
    {
        if (SFXOn)
        {
            SoundManager.Inst.setSFXVolume(0);
            SFXOn = false;
            Image_SFX.sprite = TogleImages[1];
        }
        else
        {
            SoundManager.Inst.setSFXVolume(1);
            SFXOn = true;
            Image_SFX.sprite = TogleImages[0];
        }
        SoundManager.Inst.PlaySFX("SFX_ClickBtn");
    }

    public void Btn_Close()
    {
        UIPanel.SetActive(false); 
        Time.timeScale = 1f;
        SoundManager.Inst.PlaySFX("SFX_ClickBtn");
    }

    public void Btn_MoveToTown()
    {
#if UNITY_ANDROID
        if (!IsAppInstalled(TownPackageName))
        {
            Debug.Log("���� ������� �ʽ��ϴ�");
            return;
        }
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");

        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");

        AndroidJavaObject pm = jo.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject intent = pm.Call<AndroidJavaObject>("getLaunchIntentForPackage", TownPackageName);

        jo.Call("startActivity", intent);


        Application.Quit();
#else
        Debug.Log("NOT SUPPORTED IN EDITOR");
#endif
    }
    bool IsAppInstalled(string bundleID)
    {
#if UNITY_ANDROID
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject packageManager = currentActivity.Call<AndroidJavaObject>("getPackageManager");

        AndroidJavaObject launchIntent = null;

        //if the app is installed, no errors. Else, doesn't get past next line
        try
        {
            launchIntent = packageManager.Call<AndroidJavaObject>("getLaunchIntentForPackage", bundleID);
        }
        catch (System.Exception ex)
        {
            Debug.Log("exception" + ex.Message);
            //���⿡�� ���� ��ġ ���� �ʾ������� ����ó��.
        }

        return (launchIntent == null ? false : true);
#else

        Debug.LogError("NOT SUPPORTED IN EDITOR");
        return false;
#endif

    }

}
