/****************************************************
    文件：GameDataManager.cs
	作者：积极向上小木木
    邮箱：positivemumu@126.com
    日期：2020/11/25 16:34:59
	功能：游戏运行数据管理类
*****************************************************/

using UnityEngine;
using LitJson;
using System.IO;
using System;

public class GameDataManager :MonoBehaviour
{

    public GameData gameData;
    public WeaponTypes weaponType;
    public int _arrow;
    public Action<int,int> UpdateMapSize;

    private static GameDataManager _instance;
    public static GameDataManager Instance
    {
        get => _instance;
    }
    private bool _showContinue = true;
    
    public void Awake()
    {
        _instance = this;
        LoadGameData();
        DontDestroyOnLoad(gameObject);
    }

    public void NewGame()
    {
        if(File.Exists(Consts.AssetsPath))
        {
            File.Delete(Consts.AssetsPath);
        }
        LoadGameData();
    }

    public bool IsFirstGame()
    {
        return !_showContinue;
    }
    public void SaveGameData()
    {
        string text = JsonMapper.ToJson(gameData);
        if(File.Exists(Consts.AssetsPath))
        {
            File.Delete(Consts.AssetsPath);
        }

        File.WriteAllText(Consts.AssetsPath, text);
    }

    private void LoadGameData()
    {
        string text = "";
        if (!File.Exists(Consts.AssetsPath))
        {
            TextAsset textAsset = Resources.Load<TextAsset>(Consts.GameDataName);
            File.WriteAllText(Consts.AssetsPath, textAsset.text);
            _showContinue = false;
        }
        else
        {
            _showContinue = true;
        }
        text = File.ReadAllText(Consts.AssetsPath);
        gameData = JsonMapper.ToObject<GameData>(text);
        gameData.Init();
        
        //Debug.Log(gameData.ToString());
    }

    public Vector2 GetMapSize()
    {
        return new Vector2(gameData.Width, gameData.Height);
    }

    private void OnDestroy()
    {
        SaveGameData();
    }
}