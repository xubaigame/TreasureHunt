/****************************************************
    文件：MapManager.cs
	作者：积极向上小木木
    邮箱: positivemumu@126.com
    日期：2020/11/25 19:3:42
	功能：地图管理类
*****************************************************/

using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour 
{
    [Header("元素预制体")]
    //左下、右下、左上、右上、上、下、左、右
    public GameObject[] MapBorder;
    public GameObject MapBackGround;
    public Sprite[] Tiles;
    public Sprite[] Numbers;
    public Sprite[] Traps;
    public GameObject FlagElement;
    public GameObject FlagEffect;
    public GameObject BaseElement;
    public GameObject UncoveredEffect;

    [Header("关卡设置")]
    public float MinTrapProbability;
    public float MaxTrapProbability;

    private Transform _mapHolder;
    private BaseElement[,] _map;
    private int _mapWidth;
    private int _mapHeight;

    private static MapManager _instance = null;
    public static MapManager Instance
    {
        get => _instance;
    }
    private void Awake()
    {
        _instance = this;
        Vector2 temp = GameDataManager.Instance.GetMapSize();
        _mapWidth = (int)temp.x;
        _mapHeight = (int)temp.y;
        GameDataManager.Instance.UpdateMapSize += UpdateMapSize;
        _map = new BaseElement[_mapWidth, _mapHeight];
    }
    public void Start()
    {
        InitCamera();
        _mapHolder = GameObject.Find("Map").transform;
        CreateMap();
    }
    public void UpdateMapSize(int mapWidth, int mapHeight)
    {
        _mapWidth = mapWidth;
        _mapHeight = mapHeight;
    }
    /// <summary>
    /// 初始化摄像机，使其对准屏幕中心。
    /// </summary>
    public void InitCamera()
    {
        Camera.main.orthographicSize = (_mapHeight + 3) / 2f;
        Camera.main.transform.position = new Vector3((_mapWidth - 1) / 2f, (_mapHeight - 1) / 2f, -10);
    }

    /// <summary>
    /// 创建地图
    /// </summary>
    public void CreateMap()
    {
        CreateBorderAndBackGround();
        List<int> availableIndex = new List<int>();
        for (int i = 0; i < _mapWidth*_mapHeight; i++)
        {
            availableIndex.Add(i);
        }
        GenerateTrapElement(availableIndex);
        GenerateNumberElement(availableIndex);
    }

    /// <summary>
    /// 创建边界和背景
    /// </summary>
    private void CreateBorderAndBackGround()
    {
        for (int i = 0; i < _mapWidth; i++)
        {
            for (int j = 0; j < _mapHeight; j++)
            {
                Instantiate(MapBackGround, new Vector3(i, j, 0), Quaternion.identity).transform.parent= _mapHolder;
                _map[i, j] = Instantiate(BaseElement, new Vector3(i, j, 0), Quaternion.identity, _mapHolder).GetComponent<BaseElement>();
            }
        }
        Instantiate(MapBorder[0], new Vector3(-1.25f, -1.25f, 0), Quaternion.identity).transform.parent = _mapHolder;
        Instantiate(MapBorder[1], new Vector3(_mapWidth+0.25f, -1.25f, 0), Quaternion.identity).transform.parent = _mapHolder;
        Instantiate(MapBorder[2], new Vector3(-1.25f, _mapHeight+0.25f, 0), Quaternion.identity).transform.parent = _mapHolder;
        Instantiate(MapBorder[3], new Vector3(_mapWidth + 0.25f, _mapHeight + 0.25f, 0), Quaternion.identity).transform.parent = _mapHolder;
        for (int i=0;i<_mapWidth;i++)
        {
            Instantiate(MapBorder[4], new Vector3(i, _mapHeight + 0.25f, 0), Quaternion.identity).transform.parent = _mapHolder;
        }
        for (int i = 0; i < _mapWidth; i++)
        {
            Instantiate(MapBorder[5], new Vector3(i, -1.25f, 0), Quaternion.identity).transform.parent = _mapHolder;
        }
        for (int i = 0; i < _mapHeight; i++)
        {
            Instantiate(MapBorder[6], new Vector3(-1.25f, i, 0), Quaternion.identity).transform.parent = _mapHolder;
        }
        for (int i = 0; i < _mapHeight; i++)
        {
            Instantiate(MapBorder[7], new Vector3(_mapWidth + 0.25f, i, 0), Quaternion.identity).transform.parent = _mapHolder;
        }
    }

    /// <summary>
    /// 初始化陷阱元素
    /// </summary>
    /// <param name="availableIndex">尚未初始化的元素索引</param>
    private void GenerateTrapElement(List<int> availableIndex)
    {
        float trapProbability = Random.Range(MinTrapProbability, MaxTrapProbability);
        int trapCount = (int)(availableIndex.Count * trapProbability);

        for (int i = 0; i < trapCount; i++)
        {
            int tempIndex = availableIndex[Random.Range(0, availableIndex.Count)];
            int positionX, positionY;
            GameTool.Instance.IndexToPositionXAndPositionY(tempIndex,out positionX, out positionY);
            ChangeElementType(tempIndex, ElementContents.Trap);
            availableIndex.Remove(tempIndex);
        }
    }

    /// <summary>
    /// 初始化数字元素
    /// </summary>
    /// <param name="availableIndex">尚未初始化的元素索引</param>
    private void GenerateNumberElement(List<int> availableIndex)
    {
        foreach (var item in availableIndex)
        {
            ChangeElementType(item, ElementContents.Number);
        }
        availableIndex.Clear();
    }


    
    

    /// <summary>
    /// 计算位置周围（3*3）的陷阱个数
    /// </summary>
    /// <param name="positionX">位置横坐标</param>
    /// <param name="positionY">位置纵坐标</param>
    /// <returns>陷阱个数</returns>
    public int GetTrapCountAroundElement(int positionX,int positionY)
    {
        int count = 0;
        for (int i = positionX - 1; i <= positionX + 1; i++)
        {
            for (int j = positionY-1; j <= positionY + 1; j++)
            {
                if (IsSameContnet(i, j, ElementContents.Trap))
                {
                    count++;
                }
            }
        }
        return count;
    }

    /// <summary>
    /// 位置类型判断
    /// </summary>
    /// <param name="positionX">位置横坐标</param>
    /// <param name="positionY">位置纵坐标</param>
    /// <param name="elementContent">判断类型</param>
    /// <returns>判断结果</returns>
    private bool IsSameContnet(int positionX,int positionY,ElementContents elementContent)
    {
        if (GameTool.Instance.IsPositionValid(positionX, positionY))
        {
            return _map[positionX, positionY].elementContent == elementContent;
        }
        return false;
    }

    /// <summary>
    /// 泛洪算法翻开周围元素
    /// </summary>
    /// <param name="positionX">位置横坐标</param>
    /// <param name="positionY">位置纵坐标</param>
    /// <param name="visited">访问路径数组</param>
    public void FloodingElement(int positionX, int positionY, bool[,] visited)
    {
        if (GameTool.Instance.IsPositionValid(positionX, positionY))
        {
            if (visited[positionX, positionY]) return;
            if (_map[positionX, positionY].elementType == ElementTypes.CantCovered) return;
            if(_map[positionX,positionY].elementState==ElementStates.Covered)
            {
                ((SingleCoverElement)_map[positionX, positionY]).UncovredElementFirst();
            }
            if (GetTrapCountAroundElement(positionX, positionY) > 0) return;
            visited[positionX, positionY] = true;
            for (int i = positionX - 1; i <= positionX + 1; i++)
            {
                for (int j = positionY - 1; j <= positionY + 1; j++)
                {
                    if (GameTool.Instance.IsPositionValid(i,j))
                    {
                        FloodingElement(i, j, visited);
                    }
                }
            }
        }
    }

    /// <summary>
    /// 翻开位置周围元素
    /// </summary>
    /// <param name="positionX">位置横坐标</param>
    /// <param name="positionY">位置纵坐标</param>
    public void UncoveredAdjacentElements(int positionX,int positionY)
    {
        int mark = 0;
        for (int i = positionX - 1; i <= positionX + 1; i++)
        {
            for (int j = positionY - 1; j <= positionY + 1; j++)
            {
                if (GameTool.Instance.IsPositionValid(i, j))
                {
                    if (_map[i, j].elementState == ElementStates.Marked) mark++;
                    else if (_map[i, j].elementState == ElementStates.Uncovered && _map[i, j].elementContent == ElementContents.Trap)
                        mark++;
                }
            }
        }

        if(GetTrapCountAroundElement(positionX,positionY)==mark)
        {
            for (int i = positionX - 1; i <= positionX + 1; i++)
            {
                for (int j = positionY - 1; j <= positionY + 1; j++)
                {
                    if (GameTool.Instance.IsPositionValid(i, j))
                    {
                        _map[i, j].OnPlayerStand();
                    }
                }
            }
        }
    }
    

    /// <summary>
    /// 设置位置元素的类型
    /// </summary>
    /// <param name="index">要设置元素的位置</param>
    /// <param name="content">元素类型</param>
    /// <returns>设置后的元素</returns>
    private BaseElement ChangeElementType(int index,ElementContents content)
    {
        int positionX, positionY;
        GameTool.Instance.IndexToPositionXAndPositionY(index, out positionX, out positionY);
        GameObject temp = _map[positionX, positionY].gameObject;
        Destroy(temp.GetComponent<BaseElement>());
        switch (content)
        {
            case ElementContents.Number:
                _map[positionX, positionY] = temp.AddComponent<NumberElement>();
                return _map[positionX, positionY];
            case ElementContents.Trap:
                _map[positionX, positionY]=temp.AddComponent<TrapElement>();
                return _map[positionX, positionY];
            case ElementContents.Tool:
                break;
            case ElementContents.Gold:
                break;
            case ElementContents.Enemy:
                break;
            case ElementContents.Door:
                break;
            case ElementContents.BigWall:
                break;
            case ElementContents.SmallWall:
                break;
            case ElementContents.Exit:
                break;
        }
        return null;
    }

    /// <summary>
    /// 显示所有陷阱
    /// </summary>
    public void ShowAllTrap()
    {
        for (int i = 0; i < _mapWidth; i++)
        {
            for (int j = 0; j < _mapHeight; j++)
            {
                if(_map[i,j].elementContent==ElementContents.Trap)
                {
                    ((SingleCoverElement)_map[i, j]).UncovredElementFirst();
                }
            }
        }
    }
}