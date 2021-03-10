/****************************************************
    文件：GameStartWindow.cs
	作者：积极向上小木木
    邮箱：positivemumu@126.com
    日期：2020/11/25 16:32:32
	功能：游戏开始界面
*****************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

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
        GameDataManager.Instance.NewGame();
        EnterNextScene();
    }

    public void OnLoadGameButtonDown()
    {
        EnterNextScene();
    }

    public void OnQuitGameButtonDown()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
    Application.Quit();
# endif
    }

    private void EnterNextScene()
    {
        SceneManager.LoadScene(1);
    }
}