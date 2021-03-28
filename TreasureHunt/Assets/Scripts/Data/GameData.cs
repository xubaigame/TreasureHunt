/****************************************************
    文件：GameData.cs
	作者：积极向上小木木
    邮箱：positivemumu@126.com
    日期：2020/11/25 16:35:28
	功能：游戏运行数据类
*****************************************************/

using UnityEngine;


public class GameData 
{
    private int _baseWidth;
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
    
    public int BaseWidth { get => _baseWidth; set => _baseWidth = value; }
    public int Width { get => _width; set => _width = value; }
    public int Height { get => _height; set => _height = value; }
    public int Level { get => _level; set => _level = value; }
    public int Hp { get => _hp; set => _hp = value; }
    public int Armor { get => _armor; set => _armor = value; }
    public int Key { get => _key; set => _key = value; }
    public int Hoe { get => _hoe; set => _hoe = value; }
    public int Tnt { get => _tnt; set => _tnt = value; }
    public int Map { get => _map; set => _map = value; }
    public int Gold { get => _gold; set => _gold = value; }


    public void Init()
    {
        _width = _baseWidth + (_level - 1) * 3;
        GameTool.Instance.UpdateMapSize();
    }

    
}