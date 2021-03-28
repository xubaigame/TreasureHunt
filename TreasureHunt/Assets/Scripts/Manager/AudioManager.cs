using System;
using System.Collections.Generic;
using System.Text;
using Unity.Collections;
using UnityEngine;


public class AudioManager : MonoBehaviour
{

    public string ResourceDir = "";
    
    private static AudioManager instance = null;
    private AudioSource m_bgSound;
    private AudioSource m_effectSound;
    private bool mute = false;

    public static AudioManager Instance
    {
        get => instance;
    }
    protected void Awake()
    {
        instance = this;
        m_bgSound = this.gameObject.AddComponent<AudioSource>();
        m_bgSound.playOnAwake = false;
        m_bgSound.loop = true;
        m_effectSound = this.gameObject.AddComponent<AudioSource>();
        SetMuteState(GameDataManager.Instance.isMute);
        GameDataManager.Instance.MuteChange += SetMuteState;
    }

    public void SetMuteState(bool state)
    {
        mute = state;
        if (mute)
        {
            m_bgSound.volume = 0;
            m_effectSound.volume = 0;
        }
        else
        {
            m_bgSound.volume = 1;
            m_effectSound.volume = 1;
        }
    }
    //音乐大小
    public float BgVolume
    {
        get { return m_bgSound.volume; }
        set { m_bgSound.volume = value; }
    }

    //音效大小
    public float EffectVolume
    {
        get { return m_effectSound.volume; }
        set { m_effectSound.volume = value; }
    }

    //播放音乐
    public void PlayBg(string audioName)
    {
        //当前正在播放的音乐文件
        string oldName;
        if (m_bgSound.clip == null)
            oldName = "";
        else
            oldName = m_bgSound.clip.name;

        if (oldName != audioName)
        {
            //音乐文件路径
            string path;
            if (string.IsNullOrEmpty(ResourceDir))
                path = audioName;
            else
                path = ResourceDir + "/" + audioName;

            //加载音乐
            AudioClip clip = Resources.Load<AudioClip>(path);

            //播放
            if(clip!=null)
            {
                m_bgSound.clip = clip;
                m_bgSound.Play();
            }
        }
    }

    //停止音乐
    public void StopBg()
    {
        m_bgSound.Stop();
        m_bgSound.clip = null;
    }

    //播放音效
    public void PlayEffect(string audioName)
    {
        //路径
        string path;
        if (string.IsNullOrEmpty(ResourceDir))
            path = audioName;
        else
            path = ResourceDir + "/" + audioName;

        //音频
        AudioClip clip = Resources.Load<AudioClip>(path);

        //播放
        m_effectSound.PlayOneShot(clip);
    }   
}