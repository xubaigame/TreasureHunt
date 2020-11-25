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
    public GameObject FlagElement;
    public GameObject BaseElement;


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
}