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
        GameDataManager.Instance.UpdateMapSize += UpdateMapSize;
        map = new BaseElement[mapWidth, mapHeight];
    }
    public void Start()
    {
        InitCamera();
        mapHolder = GameObject.Find("Map").transform;
        CreateMap();
    }
    
    /// <summary>
    /// 地图数据变化更新函数
    /// </summary>
    /// <param name="mapWidth">地图长度</param>
    /// <param name="mapHeight">地图宽度</param>
    public void UpdateMapSize(int mapWidth, int mapHeight)
    {
        this.mapWidth = mapWidth;
        this.mapHeight = mapHeight;
    }
    
    /// <summary>
    /// 初始化摄像机，使其对准屏幕中心。
    /// </summary>
    public void InitCamera()
    {
        Camera.main.orthographicSize = (mapHeight + 3) / 2f;
        Camera.main.transform.position = new Vector3((mapWidth - 1) / 2f, (mapHeight - 1) / 2f, -10);
    }

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
        GenerateExitElement(availableIndex);
        GenerateTrapElement(availableIndex);
        GenerateNumberElement(availableIndex);
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
                GenerateCloseObstacleArea(availableIndex);
            }
            else
            {
                GenerateRandomObstacleArea(i,availableIndex);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="nowIndex"></param>
    /// <param name="availableIndex"></param>
    public void GenerateCloseObstacleArea(int nowIndex, List<int> availableIndex)
    {
        Obstacle obstacle = new Obstacle();
        obstacle.type = (ObstacleTypes)Random.Range(0, 4);
        obstacle.doorType = (ObstacleDoorTypes)Random.Range(3, 7);
        switch (obstacle.type)
        {
            case ObstacleTypes.Self:
                int width = Random.Range(3, LevelData.ObstacleWidth - 1);
                int height = Random.Range(3, mapHeight - 1);

                obstacle.sx = LevelData.StandAreaWidth + LevelData.ObstacleWidth * nowIndex + 1 + Random.Range(0, LevelData.ObstacleWidth - 1 - width);
                obstacle.sy = Random.Range(0, mapHeight - 1 - height);

                obstacle.ex = obstacle.sx + width;
                obstacle.ey = obstacle.sy + height;

                obstacle.goldNums = Mathf.RoundToInt((width - 2) * (height - 2) * Random.Range(0.5f, 0.7f));
                
                obstacle.doorType = ObstacleDoorTypes.BigWall;

                //todo 生成墙体

                GenerateCloseAreaRewards(obstacle, availableIndex);
                GenerateCloseAreaTool(obstacle, availableIndex);
                break;
            case ObstacleTypes.WithOneWall:
                WithOneWallArea();
                break;
            case ObstacleTypes.WithTwoWall:
                WithTwoWallArea();
                break;
        }
    }

    public void GenerateCloseAreaRewards(Obstacle obstacle, List<int> availableIndex)
    {

    }

    /// <summary>
    /// 闭合障碍物区域道具生成
    /// </summary>
    /// <param name="obstacle"></param>
    /// <param name="availableIndex"></param>
    public void GenerateCloseAreaTool(Obstacle obstacle, List<int> availableIndex)
    {
        obstacle.tx = Random.Range(0, obstacle.sx);
        obstacle.ty = Random.Range(0, mapHeight);

        if (!availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.tx, obstacle.ty))) ;
        {
            obstacle.tx = Random.Range(0, obstacle.sx);
            obstacle.ty = Random.Range(0, mapHeight);
        }
        availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.tx, obstacle.ty)) ;
        ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(obstacle.tx, obstacle.ty), ElementContents.Tool);
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
            if (!availableIndex.Contains(GameTool.Instance.PositionXAndPositionYToIndex(tempx, tempy))) ;
            {
                tempx = Random.Range(start, end);
                tempy = Random.Range(0, mapHeight);
            }
            ChangeElementType(GameTool.Instance.PositionXAndPositionYToIndex(tempx, tempy), ElementContents.SmallWall);
            availableIndex.Remove(GameTool.Instance.PositionXAndPositionYToIndex(tempx, tempy));
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
            for (int i = positionX - 1; i <= positionX + 1; i++)
            {
                for (int j = positionY - 1; j <= positionY + 1; j++)
                {
                    if (GameTool.Instance.IsPositionValid(i, j))
                    {
                        map[i, j].OnPlayerStand();
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
                break;
            case ElementContents.Gold:
                map[positionX, positionY]=temp.AddComponent<GoldElement>();
                return map[positionX, positionY];
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
                map[positionX, positionY] = temp.AddComponent<ExitElement>();
                return map[positionX, positionY];
                break;
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
                if(map[i,j].elementContent==ElementContents.Trap)
                {
                    ((SingleCoverElement)map[i, j]).UncovredElementFirst();
                }
            }
        }
    }
    
    public void ChangeToNumberElement(BaseElement baseElement,bool needEffect)
    {
        map[baseElement.PositionX, baseElement.PositionY] = baseElement.gameObject.AddComponent<NumberElement>();
        ((NumberElement) map[baseElement.PositionX, baseElement.PositionY]).needEffect = needEffect;
        map[baseElement.PositionX, baseElement.PositionY].OnPlayerStand();
        Destroy(baseElement);
    }
}