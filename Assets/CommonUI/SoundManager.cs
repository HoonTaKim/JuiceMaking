using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Inst;

    [SerializeField] AudioClip[] bgm = null;
    [SerializeField] AudioClip[] sfx = null;

    [SerializeField] AudioSource bgmPlayer = null;
    [SerializeField] AudioSource[] sfxPlayer = null;

    private void Awake()
    {
        if (Inst != null)
        {
            Destroy(gameObject);
            return;
        }

        Inst = this;
        DontDestroyOnLoad(gameObject);
    }
    public void PlayBGM(string p_bgmName)
    {
        for (int i = 0; i < bgm.Length; i++)
        {
            if (p_bgmName.Equals(bgm[i].name))
            {
                bgmPlayer.clip = bgm[i];
                bgmPlayer.Play();
            }
        }
    }

    public void StopBGM()
    {
        bgmPlayer.Stop();
    }

    public void PlaySFX(string p_sfxName)
    {
        for (int i = 0; i < sfx.Length; i++)
        {
            if (p_sfxName.Equals(sfx[i].name))
            {
                for (int j = 0; j < sfxPlayer.Length; j++)
                {
                    // SFXPlayer에서 재생 중이지 않은 Audio Source를 발견했다면 
                    if (!sfxPlayer[j].isPlaying)
                    {
                        sfxPlayer[j].clip = sfx[i];
                        sfxPlayer[j].Play();
                        return;
                    }
                }
                return;
            }
        }
        Debug.Log(p_sfxName + " 이름의 효과음이 없습니다.");
        return;
    }

    public void StopSFX()
    {
        for (int i = 0; i < sfxPlayer.Length; i++)
        {
            sfxPlayer[i].Stop();
        }
    }

    public void setBGMVolume(float value)
    {
        bgmPlayer.volume = value;
    }
    public void setSFXVolume(float value)
    {
        for(int i = 0; i < sfxPlayer.Length; i++)
        {
            sfxPlayer[i].volume = value;
        }
    }
}
