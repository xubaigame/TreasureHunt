/****************************************************
    文件：GameData.cs
	作者：积极向上小木木
    邮箱: positivemumu@126.com
    日期：2020/11/25 16:35:28
	功能：游戏运行数据类
*****************************************************/

using UnityEngine;

public class GameData 
{
    private bool _isFirstGame;
    private bool _isMute;
    private int _width;
    private int _height;
    private int _level;
    private int _hp;
    private int _armor;
    private int _key;
    private int _hoe;
    private int _tnt;
    private int _map;
    private int _gold;
    private bool _grass;

    public bool IsMute { get => _isMute;}
    public int Width { get => _width;}
    public int Height { get => _height;}
    public int Level { get => _level;}
    public int Hp { get => _hp;}
    public int Armor { get => _armor;}
    public int Key { get => _key;}
    public int Hoe { get => _hoe;}
    public int Tnt { get => _tnt;}
    public int Map { get => _map;}
    public int Gold { get => _gold;}
    public bool IsFirstGame { get => _isFirstGame;}
    public bool Grass { get => _grass;}

    public void LoadData()
    {
        _isMute = PlayerPrefs.GetInt(Consts.P_IsMute, 0) == 0 ? false : true;
        if(PlayerPrefs.HasKey(Consts.P_IsFirstGame))
        {
            _isFirstGame = false;
        }
        else
        {
            _isFirstGame = true;
        }
        _level = PlayerPrefs.GetInt(Consts.P_Level, 1);
        _hp = PlayerPrefs.GetInt(Consts.P_Hp, 3);
        _armor = PlayerPrefs.GetInt(Consts.P_Armor, 0);
        _key = PlayerPrefs.GetInt(Consts.P_Key, 0);
        _hoe = PlayerPrefs.GetInt(Consts.P_Hoe, 0);
        _tnt = PlayerPrefs.GetInt(Consts.P_Tnt, 0);
        _map = PlayerPrefs.GetInt(Consts.P_Map, 0);
        _gold = PlayerPrefs.GetInt(Consts.P_Gold, 0);
        _grass = false;
    }
    public void ResetData(bool isReloadData)
    {
        _level = 1;
        _hp = 3;
        _armor = 0;
        _key = 0;
        _hoe = 0;
        _tnt = 0;
        _map = 0;
        _gold = 0;
        SaveData();
        if (isReloadData)
            LoadData();
    }
    public void SetMuteState(bool isMute)
    {
        _isMute = isMute;
        PlayerPrefs.SetInt(Consts.P_IsMute, _isMute == false ? 0 : 1);
    }
    public void SaveData()
    {
        if (!PlayerPrefs.HasKey(Consts.P_IsFirstGame))
        {
            PlayerPrefs.SetInt(Consts.P_IsFirstGame, 1);
        }
        PlayerPrefs.SetInt(Consts.P_IsFirstGame, IsFirstGame ? 0 : 1);
        PlayerPrefs.SetInt(Consts.P_Level, _level);
        PlayerPrefs.SetInt(Consts.P_Hp, _hp);
        PlayerPrefs.SetInt(Consts.P_Armor, _armor);
        PlayerPrefs.SetInt(Consts.P_Key, _key);
        PlayerPrefs.SetInt(Consts.P_Hoe, _hoe);
        PlayerPrefs.SetInt(Consts.P_Tnt, _tnt);
        PlayerPrefs.SetInt(Consts.P_Map, _map);
        PlayerPrefs.SetInt(Consts.P_Gold, _gold);

        //PlayerPrefs.DeleteAll();
    }
}