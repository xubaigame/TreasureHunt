// ****************************************************
//     文件：GameWinWindow.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/27 22:32:16
//     功能：游戏胜利界面
// *****************************************************

using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameWinWindow : MonoBehaviour
{
    public void OnNextLevelButtonDown()
    {
        AudioManager.Instance.PlayEffect(Consts.button);
        SceneManager.LoadScene(1);
    }

    public void OnExitButtonDown()
    {
        AudioManager.Instance.PlayEffect(Consts.button);
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
    Application.Quit();
# endif
    }
}
