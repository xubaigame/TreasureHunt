/****************************************************
    文件：GameDataManager.cs
	作者：积极向上小木木
    邮箱: positivemumu@126.com
    日期：2020/11/25 16:34:59
	功能：游戏运行数据管理类
*****************************************************/

using UnityEngine;

public class GameDataManager :MonoBehaviour
{
    public GameData gameData;
    private static GameDataManager _instance;
    public static GameDataManager Instance
    {
        get => _instance;
    }
    public void Awake()
    {
        _instance = this;
        gameData = new GameData();
        gameData.LoadData();
        DontDestroyOnLoad(gameObject);
    }

    public void NewGame()
    {
        gameData.ResetData(true);
    }

    public bool IsFirstGame()
    {
        return gameData.IsFirstGame;
    }

}