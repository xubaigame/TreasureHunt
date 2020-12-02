/****************************************************
    文件：MapManager.cs
	作者：积极向上小木木
    邮箱: positivemumu@126.com
    日期：2020/11/25 19:3:42
	功能：地图管理类
*****************************************************/

using UnityEngine;
using UnityEngine.UIElements;

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


    [Header("地图设置")]
    public int Width;
    public int Height;

    private Transform _mapHolder;

    private BaseElement[,] _map;

    private static MapManager _instance = null;
    public static MapManager Instance
    {
        get => _instance;
    }
    private void Awake()
    {
        _instance = this;
        _map = new BaseElement[Width, Height];
    }
    public void Start()
    {
        InitCamera();
        _mapHolder = GameObject.Find("Map").transform;
        CreateMap();
    }

    /// <summary>
    /// 初始化摄像机，使其对准屏幕中心。
    /// </summary>
    public void InitCamera()
    {
        Camera.main.orthographicSize = (Height + 3) / 2f;
        Camera.main.transform.position = new Vector3((Width - 1) / 2f, (Height - 1) / 2f, -10);
    }

    /// <summary>
    /// 创建地图
    /// </summary>
    public void CreateMap()
    {
        CreateBorderAndBackGround();
    }

    /// <summary>
    /// 创建边界和背景
    /// </summary>
    public void CreateBorderAndBackGround()
    {
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Instantiate(MapBackGround, new Vector3(i, j, 0), Quaternion.identity).transform.parent= _mapHolder;
                _map[i, j] = Instantiate(BaseElement, new Vector3(i, j, 0), Quaternion.identity, _mapHolder).GetComponent<BaseElement>();
            }
        }
        Instantiate(MapBorder[0], new Vector3(-1.25f, -1.25f, 0), Quaternion.identity).transform.parent = _mapHolder;
        Instantiate(MapBorder[1], new Vector3(Width+0.25f, -1.25f, 0), Quaternion.identity).transform.parent = _mapHolder;
        Instantiate(MapBorder[2], new Vector3(-1.25f, Height+0.25f, 0), Quaternion.identity).transform.parent = _mapHolder;
        Instantiate(MapBorder[3], new Vector3(Width + 0.25f, Height + 0.25f, 0), Quaternion.identity).transform.parent = _mapHolder;
        for (int i=0;i<Width;i++)
        {
            Instantiate(MapBorder[4], new Vector3(i, Height + 0.25f, 0), Quaternion.identity).transform.parent = _mapHolder;
        }
        for (int i = 0; i < Width; i++)
        {
            Instantiate(MapBorder[5], new Vector3(i, -1.25f, 0), Quaternion.identity).transform.parent = _mapHolder;
        }
        for (int i = 0; i < Height; i++)
        {
            Instantiate(MapBorder[6], new Vector3(-1.25f, i, 0), Quaternion.identity).transform.parent = _mapHolder;
        }
        for (int i = 0; i < Height; i++)
        {
            Instantiate(MapBorder[7], new Vector3(Width + 0.25f, i, 0), Quaternion.identity).transform.parent = _mapHolder;
        }
    }

    public bool IsPositionValid(int positionX,int positionY)
    {
        if (positionX >= 0 && positionX < Width && positionY >= 0 && positionY < Height)
            return true;
        return false;
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
        if (IsPositionValid(positionX, positionY))
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
        if (IsPositionValid(positionX, positionY))
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
                    if (IsPositionValid(i,j))
                    {
                        FloodingElement(i, j, visited);
                    }
                }
            }
        }
    }
}