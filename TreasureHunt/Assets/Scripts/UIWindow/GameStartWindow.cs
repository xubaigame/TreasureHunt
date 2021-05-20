/****************************************************
    文件：GameStartWindow.cs
	作者：积极向上小木木
    邮箱：positivemumu@126.com
    日期：2020/11/25 16:32:32
	功能：游戏开始界面
*****************************************************/

using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEngine.UI;

public class GameStartWindow : MonoBehaviour
{
    public GameObject LoadGameButton;
    public void Start()
    {
        if (GameDataManager.Instance.IsFirstGame())
        {
            LoadGameButton.SetActive(false);
        }
        else
        {
            LoadGameButton.SetActive(true);
        }
    }
    public void OnNewGameButtonDown()
    {
        AudioManager.Instance.PlayEffect(Consts.button);
        GameDataManager.Instance.NewGame();
        EnterNextScene();
    }

    public void OnLoadGameButtonDown()
    {
        AudioManager.Instance.PlayEffect(Consts.button);
        GameDataManager.Instance.LoadGameData();
        EnterNextScene();
    }

    public void OnQuitGameButtonDown()
    {
        AudioManager.Instance.PlayEffect(Consts.button);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
    Application.Quit();
# endif
    }

    private void EnterNextScene()
    {
        EnterCinemachineScene();
    }

    private void EnterProCameraScene()
    {
        SceneManager.LoadScene(1);
    }

    private void EnterCinemachineScene()
    {
        SceneManager.LoadScene(2);
    }
    
}