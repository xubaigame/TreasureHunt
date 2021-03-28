/****************************************************
    文件：MapManager.cs
	作者：积极向上小木木
    邮箱：positivemumu@126.com
    日期：2020/11/25 19:3:42
	功能：地图管理类
*****************************************************/

using System.Collections.Generic;
using UnityEngine;
using AStarPathfinding;

public class MapManager : MonoBehaviour
{
    public MapData mapData;
    public LevelData LevelData;

    private Transform mapHolder;
    private BaseElement[,] map;
    private int mapWidth;
    private int mapHeight;

    private static MapManager instance = null;
    public static MapManager Instance
    {
        get => instance;
    }
    private void Awake()
    {
        instance = this;
        Vector2 temp = GameDataManager.Instance.GetMapSize();
        mapWidth = (int)temp.x;
        mapHeight = (int)temp.y;
        map = new BaseElement[mapWidth, mapHeight];
        AudioManager.Instance.PlayBg(Consts.BGM);
    }
    public void Start()
    {
        CameraManager.Instance.InitCamera();
        mapHolder = GameObject.Find("Map").transform;
        CreateMap();
    }
    

    /// <summary>
    ///寻路算法
    /// </summary>
    /// <param name="targetNode">目标节点</param>
    public void FindPath(GameMapNode targetNode)
    {
        int obstacleType;
        int[,] tempMap = GetSearchMap(out obstacleType);
        AStarSystem pathfinding = new AStarSystem(tempMap,obstacleType);
        Vector2Int playerPosition = PlayerManager.Instance.GetPlayerPosition();
        GameMapNode startNode = new GameMapNode(new AStarPoint(playerPosition.x,playerPosition.y), 0);
        List<AStarNode> nodeList=new List<AStarNode>();
        bool result = pathfinding.FindPath(startNode, targetNode, ref nodeList);
        if (result)
        {
            bool tmep = false;
            foreach (var VARIABLE in nodeList)
            {
                if (VARIABLE.NodeType == 1)
                {
                    
                }
            }
            AudioManager.Instance.PlayEffect(Consts.move);
            PlayerManager.Instance.MoveWithPath(nodeList.ListToVector());
        }
        else
        {
            AudioManager.Instance.PlayEffect(Consts.why);
            PlayerManager.Instance.ShowWhyAnimation();
        }
    }

    #region 创建地图

     /// <summary>
    /// 创建地图
    /// </summary>
    public void CreateMap()
    {
        CreateBorderAndBackGround();
        List<int> availableIndex = new List<int>();
        for (int i = 0; i < mapWidth*mapHeight; i++)
        {
            availableIndex.Add(i);
        }
        
        //生成玩家站立区域
        int standy=Random.Range(0, 7);
        for (int i = 0; i < 3; i++)
        {
            for (int j = standy; j <standy+3 ; j++)
            {
                ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(i, j), ElementContents.Number);
                availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(i, j));
            }
        }
        
        GenerateObstacleAreas(availableIndex);
        GenerateExitElement(availableIndex);
        
        GenerateToolElement(availableIndex);
        GenerateGoldElement(availableIndex);
        GenerateTrapElement(availableIndex);
        GenerateNumberElement(availableIndex);
        
        //翻开玩家站立区域
        for (int i = 0; i < 3; i++)
        {
            for (int j = standy; j <standy+3 ; j++)
            {
                ((SingleCoverElement)map[i,j]).UncovredElementFirst();
            }
        }

        PlayerManager.Instance.InitPlayerPosition(1,standy+1);
    }
    
    /// <summary>
    /// 创建边界和背景
    /// </summary>
    private void CreateBorderAndBackGround()
    {
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                Instantiate(mapData.MapBackGround, new Vector3(i, j, 0), Quaternion.identity).transform.parent= mapHolder;
                map[i, j] = Instantiate(mapData.BaseElement, new Vector3(i, j, 0), Quaternion.identity, mapHolder).GetComponent<BaseElement>();
            }
        }
        Instantiate(mapData.MapBorder[0], new Vector3(-1.25f, -1.25f, 0), Quaternion.identity).transform.parent = mapHolder;
        Instantiate(mapData.MapBorder[1], new Vector3(mapWidth+0.25f, -1.25f, 0), Quaternion.identity).transform.parent = mapHolder;
        Instantiate(mapData.MapBorder[2], new Vector3(-1.25f, mapHeight+0.25f, 0), Quaternion.identity).transform.parent = mapHolder;
        Instantiate(mapData.MapBorder[3], new Vector3(mapWidth + 0.25f, mapHeight + 0.25f, 0), Quaternion.identity).transform.parent = mapHolder;
        for (int i=0;i<mapWidth;i++)
        {
            Instantiate(mapData.MapBorder[4], new Vector3(i, mapHeight + 0.25f, 0), Quaternion.identity).transform.parent = mapHolder;
        }
        for (int i = 0; i < mapWidth; i++)
        {
            Instantiate(mapData.MapBorder[5], new Vector3(i, -1.25f, 0), Quaternion.identity).transform.parent = mapHolder;
        }
        for (int i = 0; i < mapHeight; i++)
        {
            Instantiate(mapData.MapBorder[6], new Vector3(-1.25f, i, 0), Quaternion.identity).transform.parent = mapHolder;
        }
        for (int i = 0; i < mapHeight; i++)
        {
            Instantiate(mapData.MapBorder[7], new Vector3(mapWidth + 0.25f, i, 0), Quaternion.identity).transform.parent = mapHolder;
        }
    }

    /// <summary>
    /// 初始化出口元素
    /// </summary>
    /// <param name="availableIndex">尚未初始化的元素索引</param>
    private void GenerateExitElement(List<int> availableIndex)
    {
        int posY = Random.Range(0, mapHeight-1);
        int posX = mapWidth - 1;
        ExitElement exit= (ExitElement)ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(posX, posY), ElementContents.Exit);
        Destroy(exit.gameObject.GetComponent<BoxCollider2D>());
        exit.gameObject.AddComponent<BoxCollider2D>();
        exit.transform.position = new Vector3(posX - 0.5f, posY + 0.5f);
        availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(posX, posY));
        for (int i=mapWidth-2; i<mapWidth;i++)
        {
            Destroy(map[i, posY+1].gameObject);
            map[i, posY+1] = map[posX, posY];
            availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(i, posY+1));
        }
        Destroy(map[posX-1, posY].gameObject);
        map[posX-1, posY] = map[posX, posY];
        availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(posX-1, posY));

    }

    /// <summary>
    /// 生成障碍物区域
    /// </summary>
    /// <param name="availableIndex">尚未初始化的元素索引</param>
    public void GenerateObstacleAreas(List<int> availableIndex)
    {
        int obstacleNum = (mapWidth - LevelData.StandAreaWidth - 2) / LevelData.ObstacleWidth;
        for(int i=0;i<obstacleNum;i++)
        {
            if(Random.value>=0.35f)
            {
                GenerateCloseObstacleArea(i,availableIndex);
            }
            else
            {
                GenerateRandomObstacleArea(i,availableIndex);
            }
        }
    }

    /// <summary>
    /// 闭合障碍物区域生成
    /// 1-自闭合
    /// 2-与单边界闭合
    /// 3-与双边界闭合
    /// </summary>
    /// <param name="nowIndex">当前障碍区编号</param>
    /// <param name="availableIndex">尚未初始化的元素索引</param>
    public void GenerateCloseObstacleArea(int nowIndex, List<int> availableIndex)
    {
        Obstacle obstacle = new Obstacle();
        obstacle.type = (ObstacleTypes)Random.Range(0, 3);
        obstacle.doorType = (ObstacleDoorTypes)Random.Range(4, 8);
        int width = 0;
        int height = 0;
        switch (obstacle.type)
        {
            case ObstacleTypes.Self:
                
                width = Random.Range(3, LevelData.ObstacleWidth - 1);
                height = Random.Range(3, mapHeight - 1);

                obstacle.sx = LevelData.StandAreaWidth + LevelData.ObstacleWidth * nowIndex + 1 + Random.Range(0, LevelData.ObstacleWidth - 1 - width);
                obstacle.sy = 1 + Random.Range(0, mapHeight - 1 - height);

                obstacle.ex = obstacle.sx + width;
                obstacle.ey = obstacle.sy + height;

                obstacle.goldNums = Mathf.CeilToInt((width - 2) * (height - 2) * Random.Range(0.5f, 0.7f));
                
                obstacle.doorType = ObstacleDoorTypes.BigWall;

                
                for (int i = obstacle.sx; i < obstacle.ex; i++)
                {
                    if (availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(i, obstacle.sy)))
                    {
                        ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(i,obstacle.sy),ElementContents.BigWall);
                        availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(i, obstacle.sy));
                    }
                    if (availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(i, obstacle.ey - 1)))
                    {
                        ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(i, obstacle.ey - 1),
                            ElementContents.BigWall);
                        availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(i, obstacle.ey - 1));
                    }
                }
                for (int i = obstacle.sy; i < obstacle.ey; i++)
                {
                    ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.sx,i),ElementContents.BigWall);
                    ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.ex - 1, i),
                        ElementContents.BigWall);
                    availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.sx,i));
                    availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.ex - 1, i));
                }
                GenerateCloseAreaRewards(obstacle, availableIndex);
                GenerateCloseAreaTool(obstacle, availableIndex);
                break;
            case ObstacleTypes.WithOneWall:
                
                width = Random.Range(3, LevelData.ObstacleWidth - 1);
                height = Random.Range(3, mapHeight - 1);

                obstacle.sx = LevelData.StandAreaWidth + LevelData.ObstacleWidth * nowIndex + 1 + Random.Range(0, LevelData.ObstacleWidth - 1 - width);
                obstacle.ex = obstacle.sx + width;
                obstacle.ey = height;
                obstacle.goldNums = Mathf.CeilToInt((width - 2) * (height - 1) * Random.Range(0.5f, 0.7f));
                WithOneWallArea(obstacle,availableIndex);
                break;
            case ObstacleTypes.WithTwoWall:
                width = Random.Range(3, LevelData.ObstacleWidth - 1);
                height = Random.Range(3, mapHeight - 1);

                obstacle.sx = LevelData.StandAreaWidth + LevelData.ObstacleWidth * nowIndex + 1 + Random.Range(0, LevelData.ObstacleWidth - 1 - width);
                obstacle.ex = obstacle.sx + width;
                obstacle.ey = height;
                
                obstacle.goldNums = Mathf.CeilToInt((width - 2) * (height - 1) * Random.Range(0.5f, 0.7f));
                
                WithTwoWallArea(obstacle,availableIndex);
                break;
        }
    }

    /// <summary>
    /// 与单边界闭合障碍物生成
    /// 1-与上边界闭合
    /// 2-与下边界闭合
    /// </summary>
    /// <param name="obstacle">闭合区域信息</param>
    /// <param name="availableIndex">尚未初始化的元素索引</param>
    public void WithOneWallArea(Obstacle obstacle,List<int> availableIndex)
    {
        if (Random.value >=0.5f)
        {
            obstacle.dp = Random.value > 0.5f
                ? new Vector2(Random.Range(obstacle.sx, obstacle.ex), obstacle.ey - 1)
                : new Vector2(Random.value>0.5?obstacle.sx:obstacle.ex-1, Random.Range(0, obstacle.ey));
            ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex((int)obstacle.dp.x, (int)obstacle.dp.y),(ElementContents)(int)obstacle.doorType);
            availableIndex.Remove(
                GameTool.Instance.PositionXAndPositionYToIndex((int) obstacle.dp.x, (int) obstacle.dp.y));
            for (int i = 0; i < obstacle.ey; i++)
            {
                if (availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.sx, i)))
                {
                    ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.sx,i),ElementContents.BigWall);
                    availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.sx,i));
                }
            }
            for (int i = obstacle.sx; i < obstacle.ex; i++)
            {
                if (availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(i, obstacle.ey - 1)))
                {
                    ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(i, obstacle.ey - 1), ElementContents.BigWall);
                    availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(i, obstacle.ey - 1));
                }
            }

            for (int i = 0; i < obstacle.ey; i++)
            {
                if (availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.ex - 1, i)))
                {
                    ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.ex - 1, i),
                        ElementContents.BigWall);
                    availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.ex - 1, i));
                }
            }
            obstacle.sy = 0;
            GenerateCloseAreaRewards(obstacle,availableIndex);
        }
        else
        {
            obstacle.dp = Random.value > 0.5f
                ? new Vector2(Random.Range(obstacle.sx + 1, obstacle.ex-1), obstacle.ey-1)
                : new Vector2(Random.value>0.5?obstacle.sx:obstacle.ex-1, Random.Range(obstacle.ey-1, mapHeight));
            ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex((int)obstacle.dp.x, (int)obstacle.dp.y),(ElementContents)(int)obstacle.doorType);
            availableIndex.Remove(
                GameTool.Instance.PositionXAndPositionYToIndex((int) obstacle.dp.x, (int) obstacle.dp.y));
            
            for (int i = obstacle.ey; i < mapHeight; i++)
            {
                if (availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.sx, i)))
                {
                    ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.sx,i),ElementContents.BigWall);
                    availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.sx,i));
                }
            }
            for (int i = obstacle.sx; i < obstacle.ex; i++)
            {
                if (availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(i, obstacle.ey - 1)))
                {
                    ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(i, obstacle.ey - 1), ElementContents.BigWall);
                    availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(i, obstacle.ey - 1));
                }
            }

            for (int i = obstacle.ey; i < mapHeight; i++)
            {
                if (availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.ex - 1, i)))
                {
                    ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.ex - 1, i),
                        ElementContents.BigWall);
                    availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.ex - 1, i));
                }
            }

            obstacle.sy = mapHeight - obstacle.ey + 1;
            obstacle.ey = mapHeight + 1;
            GenerateCloseAreaRewards(obstacle,availableIndex);
        }
        GenerateCloseAreaTool(obstacle,availableIndex);
    }

    /// <summary>
    /// 与上下边界闭合
    /// </summary>
    /// <param name="obstacle">闭合区域信息</param>
    /// <param name="availableIndex">尚未初始化的元素索引</param>
    public void WithTwoWallArea(Obstacle obstacle,List<int> availableIndex)
    {
        if (Random.value >=0.5f)
        {
            obstacle.dp = Random.value > 0.5f
                ? new Vector2(Random.Range(obstacle.sx, obstacle.ex), obstacle.ey - 1)
                : Random.value > 0.5
                    ? new Vector2(obstacle.sx, Random.Range(0, obstacle.ey))
                    : new Vector2(obstacle.ex - 1, Random.Range(obstacle.ey, mapHeight));
            ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex((int)obstacle.dp.x, (int)obstacle.dp.y),(ElementContents)(int)obstacle.doorType);
            availableIndex.Remove(
                GameTool.Instance.PositionXAndPositionYToIndex((int) obstacle.dp.x, (int) obstacle.dp.y));
            for (int i = 0; i < obstacle.ey; i++)
            {
                if (availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.sx, i)))
                {
                    ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.sx,i),ElementContents.BigWall);
                    availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.sx,i));
                }
            }
            for (int i = obstacle.sx; i < obstacle.ex; i++)
            {
                if (availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(i, obstacle.ey - 1)))
                {
                    ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(i, obstacle.ey - 1), ElementContents.BigWall);
                    availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(i, obstacle.ey - 1));
                }
            }

            for (int i = obstacle.ey; i < mapHeight; i++)
            {
                if (availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.ex - 1, i)))
                {
                    ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.ex - 1, i),
                        ElementContents.BigWall);
                    availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.ex - 1, i));
                }
            }
        }
        else
        {
            obstacle.dp = Random.value > 0.5f
                ? new Vector2(Random.Range(obstacle.sx, obstacle.ex), obstacle.ey - 1)
                : Random.value > 0.5
                    ? new Vector2(obstacle.sx, Random.Range(obstacle.ey, mapHeight))
                    : new Vector2(obstacle.ex - 1, Random.Range(0, obstacle.ey));
            ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex((int)obstacle.dp.x, (int)obstacle.dp.y),(ElementContents)(int)obstacle.doorType);
            availableIndex.Remove(
                GameTool.Instance.PositionXAndPositionYToIndex((int) obstacle.dp.x, (int) obstacle.dp.y));
            for (int i = obstacle.ey; i < mapHeight; i++)
            {
                if (availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.sx, i)))
                {
                    ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.sx,i),ElementContents.BigWall);
                    availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.sx,i));
                }
            }
            for (int i = obstacle.sx; i < obstacle.ex; i++)
            {
                if (availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(i, obstacle.ey - 1)))
                {
                    ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(i, obstacle.ey - 1), ElementContents.BigWall);
                    availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(i, obstacle.ey - 1));
                }
            }

            for (int i = 0; i < obstacle.ey; i++)
            {
                if (availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.ex - 1, i)))
                {
                    ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.ex - 1, i),
                        ElementContents.BigWall);
                    availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.ex - 1, i));
                }
            }
        }
        GenerateCloseAreaTool(obstacle,availableIndex);
    }

    /// <summary>
    /// 闭合障碍区奖励生成
    /// </summary>
    /// <param name="obstacle">闭合区域信息</param>
    /// <param name="availableIndex">尚未初始化的元素索引</param>
    public void GenerateCloseAreaRewards(Obstacle obstacle, List<int> availableIndex)
    {
        for (int i = 0; i < obstacle.goldNums; i++)
        {
            int tempx = Random.Range(obstacle.sx, obstacle.ex - 1);
            int tempy = Random.Range(obstacle.sy, obstacle.ey - 1);
            while(!availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(tempx, tempy)))
            {
                tempx = Random.Range(obstacle.sx, obstacle.ex - 1);
                tempy = Random.Range(obstacle.sy, obstacle.ey - 1);
            }
            GoldElement gold=(GoldElement)ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(tempx, tempy), ElementContents.Gold);
            gold.goldType = (GoldTypes)Random.Range(0, 7);
            if (gold.isHide == false)
            {
                gold.ChangeSprite();
            }
            availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(tempx, tempy));

        }
    }

    /// <summary>
    /// 闭合障碍物区域道具生成
    /// </summary>
    /// <param name="obstacle">闭合区域信息</param>
    /// <param name="availableIndex">尚未初始化的元素索引</param>
    public void GenerateCloseAreaTool(Obstacle obstacle, List<int> availableIndex)
    {
        obstacle.tx = Random.Range(0, obstacle.sx);
        obstacle.ty = Random.Range(0, mapHeight);

        while(!availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.tx, obstacle.ty)))
        {
            obstacle.tx = Random.Range(0, obstacle.sx);
            obstacle.ty = Random.Range(0, mapHeight);
        }
        availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.tx, obstacle.ty)) ;
        ToolElement tool=(ToolElement)ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.tx, obstacle.ty), ElementContents.Tool);
        tool.toolType = (ToolTypes)obstacle.doorType;
        if (tool.isHide == false)
        {
            tool.ChangeSprite();
        }
    }

    /// <summary>
    /// 随机障碍物区生成
    /// </summary>
    /// <param name="nowIndex">当前障碍区编号</param>
    /// <param name="availableIndex">尚未初始化的元素索引</param>
    public void GenerateRandomObstacleArea(int nowIndex,List<int> availableIndex)
    {
        int start = LevelData.StandAreaWidth + LevelData.ObstacleWidth * nowIndex + 1;
        int end = start + LevelData.ObstacleWidth - 2;
        for(int i=0;i<5;i++)
        {
            int tempx = Random.Range(start, end);
            int tempy = Random.Range(0, mapHeight);
            while(!availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(tempx, tempy)))
            {
                tempx = Random.Range(start, end);
                tempy = Random.Range(0, mapHeight);
            }
            ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(tempx, tempy), ElementContents.SmallWall);
            availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(tempx, tempy));
        }
    }

    /// <summary>
    /// 关卡随机道具生成
    /// </summary>
    /// <param name="availableIndex">尚未初始化的元素索引</param>
    public void GenerateToolElement(List<int> availableIndex)
    {
        for (int i = 0; i < 3; i++)
        {
            int tempIndex = availableIndex[Random.Range(0, availableIndex.Count)];
            ToolElement toolElement = (ToolElement) ChangeElementType(tempIndex, ElementContents.Tool);
            toolElement.toolType = (ToolTypes) Random.Range(0, 9);
            if (toolElement.isHide == false)
            {
                toolElement.ChangeSprite();
            }
            availableIndex.Remove(tempIndex);
        }
    }

    /// <summary>
    /// 关卡随机金币生成
    /// </summary>
    /// <param name="availableIndex">尚未初始化的元素索引</param>
    public void GenerateGoldElement(List<int> availableIndex)
    {
        for (int i = 0; i < mapWidth/4; i++)
        {
            int tempIndex = availableIndex[Random.Range(0, availableIndex.Count)];
            GoldElement goldElement = (GoldElement) ChangeElementType(tempIndex, ElementContents.Gold);
            goldElement.goldType = (GoldTypes) Random.Range(0, 7);
            if (goldElement.isHide == false)
            {
                goldElement.ChangeSprite();
            }
            availableIndex.Remove(tempIndex);
        }
    }

    /// <summary>
    /// 初始化陷阱元素
    /// </summary>
    /// <param name="availableIndex">尚未初始化的元素索引</param>
    private void GenerateTrapElement(List<int> availableIndex)
    {
        float trapProbability = Random.Range(LevelData.MinTrapProbability, LevelData.MaxTrapProbability);
        int trapCount = (int)(availableIndex.Count * trapProbability);

        for (int i = 0; i < trapCount; i++)
        {
            int tempIndex = availableIndex[Random.Range(0, availableIndex.Count)];
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

    #endregion
   

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
            return map[positionX, positionY].elementContent == elementContent;
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
            if (map[positionX, positionY].elementType == ElementTypes.CantCovered) return;
            if(map[positionX,positionY].elementState==ElementStates.Covered)
            {
                ((SingleCoverElement)map[positionX, positionY]).UncovredElementFirst();
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
    /// 翻开位置周围3*3元素（快速检查）
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
                    if (map[i, j].elementState == ElementStates.Marked) mark++;
                    else if (map[i, j].elementState == ElementStates.Uncovered && map[i, j].elementContent == ElementContents.Trap)
                        mark++;
                }
            }
        }

        if(GetTrapCountAroundElement(positionX,positionY)==mark)
        {
            PlayerManager.Instance.ShowQuickCheckAnimation();
            AudioManager.Instance.PlayEffect(Consts.quickCheck);
            for (int i = positionX - 1; i <= positionX + 1; i++)
            {
                for (int j = positionY - 1; j <= positionY + 1; j++)
                {
                    if (GameTool.Instance.IsPositionValid(i, j))
                    {
                        map[i, j].OnPlayerStand();
                        map[i, j].OnPlayerStand();
                    }
                }
            }
        }
        else
        {
            PlayerManager.Instance.ShowWhyAnimation();
            AudioManager.Instance.PlayEffect(Consts.why);
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
        GameObject temp = map[positionX, positionY].gameObject;
        Destroy(temp.GetComponent<BaseElement>());
        switch (content)
        {
            case ElementContents.Number:
                map[positionX, positionY] = temp.AddComponent<NumberElement>();
                return map[positionX, positionY];
            case ElementContents.Trap:
                map[positionX, positionY]=temp.AddComponent<TrapElement>();
                return map[positionX, positionY];
            case ElementContents.Tool:
                map[positionX, positionY]=temp.AddComponent<ToolElement>();
                return map[positionX, positionY];
            case ElementContents.Gold:
                map[positionX, positionY]=temp.AddComponent<GoldElement>();
                return map[positionX, positionY];
            case ElementContents.Enemy:
                map[positionX, positionY] = temp.AddComponent<EnemyElement>();
                return map[positionX, positionY];
            case ElementContents.Door:
                map[positionX, positionY] = temp.AddComponent<DoorElement>();
                return map[positionX, positionY];
            case ElementContents.BigWall:
                map[positionX, positionY] = temp.AddComponent<BigWallElement>();
                return map[positionX, positionY];
            case ElementContents.SmallWall:
                map[positionX, positionY] = temp.AddComponent<SmallWallElement>();
                return map[positionX, positionY];
            case ElementContents.Exit:
                map[positionX, positionY] = temp.AddComponent<ExitElement>();
                return map[positionX, positionY];
        }
        return null;
    }

    /// <summary>
    /// 显示所有陷阱
    /// </summary>
    public void ShowAllTrap()
    {
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                if (map[i, j].elementState == ElementStates.Marked && map[i, j].elementContent != ElementContents.Trap)
                {
                    map[i,j].OnRightMouseButtonDown();
                    Instantiate(mapData.ErrorElement, map[i, j].transform);
;                }
                else if(map[i, j].elementState == ElementStates.Covered&&map[i,j].elementContent==ElementContents.Trap)
                {
                    ((SingleCoverElement)map[i, j]).UncovredElementFirst();
                }
            }
        }
    }
    
    /// <summary>
    /// 将元素转换为数字元素
    /// </summary>
    /// <param name="baseElement">要转换的元素</param>
    /// <param name="needEffect">是否需要特效</param>
    public void ChangeToNumberElement(BaseElement baseElement,bool needEffect)
    {
        map[baseElement.PositionX, baseElement.PositionY] = baseElement.gameObject.AddComponent<NumberElement>();
        ((NumberElement) map[baseElement.PositionX, baseElement.PositionY]).needEffect = needEffect;
        map[baseElement.PositionX, baseElement.PositionY].OnPlayerStand();
        Destroy(baseElement);
    }
    
    /// <summary>
    /// 元素地图初始化为数字地图
    /// </summary>
    /// <param name="obstacleType">障碍物标志</param>
    /// <returns>数字地图</returns>
    public int [,] GetSearchMap(out int obstacleType)
    {
        int[,] numberMap = new int[mapWidth,mapHeight];
        //0-被翻开的数字元素
        //1-未被翻开元素
        //2-障碍元素：被标记元素、不可翻开元素
        //3-被翻开的金币或工具元素
        //4-其他元素

        obstacleType = 2;
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                if(map[i, j].elementState==ElementStates.Marked)
                {
                    numberMap[i, j] = obstacleType;
                }
                else if (map[i, j].elementState==ElementStates.Covered)
                {
                    if(map[i, j].elementContent == ElementContents.Gold||map[i, j].elementContent == ElementContents.Tool)
                    {
                        if(((DoubleCoverElement)map[i,j]).isHide==false)
                                numberMap[i, j] = 3;

                    }
                    else
                    {
                        numberMap[i, j] = 1;
                    }
                }
                else
                {
                    if (map[i, j].elementContent == ElementContents.Door ||
                        map[i, j].elementContent == ElementContents.Enemy ||
                        map[i, j].elementContent == ElementContents.BigWall ||
                        map[i, j].elementContent == ElementContents.SmallWall||
                        map[i, j].elementContent == ElementContents.Trap)
                    {
                        numberMap[i, j] = obstacleType;
                    }
                    else if (map[i, j].elementContent == ElementContents.Number||map[i, j].elementContent == ElementContents.Exit)
                    {
                        numberMap[i, j] = 0;
                    }
                    else
                    {
                        numberMap[i, j] = 4;
                    }
                        
                }
            }
        }

        return numberMap;
    }


    /// <summary>
    /// 通过位置获得地图元素内容
    /// </summary>
    /// <param name="x">横坐标</param>
    /// <param name="y">纵坐标</param>
    /// <returns>地图元素内容</returns>
    public ElementContents GetElementContentByPosition(int x, int y)
    {
        return map[x, y].elementContent;
    }

    /// <summary>
    /// 翻开元素
    /// </summary>
    /// <param name="x">横坐标</param>
    /// <param name="y">纵坐标</param>
    public void UncoverElementDouble(int x, int y)
    {
        if (map[x, y].elementState == ElementStates.Covered)
        {
            if (map[x, y] is DoubleCoverElement)
            {
                if ((map[x, y] as DoubleCoverElement).isHide == true)
                {
                    AudioManager.Instance.PlayEffect(Consts.dig);
                }
            }
            else
            {
                AudioManager.Instance.PlayEffect(Consts.dig);
            }
            
        }
        map[x,y].OnPlayerStand();
        map[x,y].OnPlayerStand();
    }

    public BaseElement GetElementByPosition(int x, int y)
    {
        if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
        {
            return map[x, y];
        }

        return null;
    }

    public void UpdateNumberAfterTntToolUsed(int x, int y)
    {

        for (int i = x - 2; i <= x + 2; i++) 
        {
            for (int j = y - 2; j <= y + 2; j++) 
            {
                if (i >= 0 && j < mapWidth && j >= 0 && j < mapHeight)
                {
                    if (map[i, j].elementState == ElementStates.Uncovered &&
                        map[i, j].elementContent == ElementContents.Number)
                    {
                        map[i,j] .LoadSprite(mapData.Numbers[GetTrapCountAroundElement(i, j)]);
                    }
                }
            }
        }
    }
    
}