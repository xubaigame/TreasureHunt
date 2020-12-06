/****************************************************
    文件：GameTool.cs
	作者：积极向上小木木
    邮箱: positivemumu@126.com
    日期：2020/12/4 21:53:5
	功能：游戏工具类
*****************************************************/



using UnityEngine;

public class GameTool 
{

    private static GameTool _instance = null;
    public static GameTool Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameTool();
            return _instance;
        }
    }

    private int _mapWidth;
    private int _mapHeight;

    public GameTool()
    {
        Vector2 temp = GameDataManager.Instance.GetMapSize();
        _mapHeight = (int)temp.y;
        _mapWidth = (int)temp.x;
        GameDataManager.Instance.UpdateMapSize += UpdateMapSize;
    }

    public void UpdateMapSize(int mapWidth,int mapHeight)
    {
        _mapWidth = mapWidth;
        _mapHeight = mapHeight;
    }

    /// <summary>
    /// 判断位置是否有效
    /// </summary>
    /// <param name="positionX">位置横坐标</param>
    /// <param name="positionY">位置纵坐标</param>
    /// <returns>是否有效</returns>
    public bool IsPositionValid(int positionX, int positionY)
    {
        
        if (positionX >= 0 && positionX < _mapWidth && positionY >= 0 && positionY < _mapHeight)
            return true;
        return false;
    }

    /// <summary>
    /// 一维索引转二维索引
    /// </summary>
    /// <param name="index">一维索引</param>
    /// <param name="positionX">二维索引横坐标</param>
    /// <param name="positionY">二维索引纵坐标</param>
    public void IndexToPositionXAndPositionY(int index, out int positionX, out int positionY)
    {
        positionY = index / _mapWidth;
        positionX = index - positionY * _mapWidth;
    }

    /// <summary>
    /// 二维索引转一维索引
    /// </summary>
    /// <param name="positionX">二维索引横坐标</param>
    /// <param name="positionY">二维索引纵坐标</param>
    /// <returns>一维索引</returns>
    public int PositionXAndPositionYToIndex(int positionX, int positionY)
    {
        return positionX + positionY * _mapWidth;
    }
}