// ****************************************************
//     文件：GameLoseWindow.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/27 22:39:43
//     功能：游戏失败界面
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoseWindow : MonoBehaviour
{
    public void OnBackButtonDown()
    {
        GameDataManager.Instance.ResetGameAsFirst();
        SceneManager.LoadScene(0);
    }
    
    public void OnExitButtonDown()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
    Application.Quit();
# endif
    }
}
