// ****************************************************
//     文件：AStarSystem.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/13 23:48:33
//     功能：AStar核心系统
// *****************************************************

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AStarPathfinding
{
    public class AStarSystem
    {
        private List<AStarNode> OpenNodeList = new List<AStarNode>();

        private List<AStarNode> CloseNodeList = new List<AStarNode>();

        private int[,] map;
        private int mapWidth;
        private int mapHeight;
        private int obstacleType;

        public AStarSystem(int[,]GameMap,int ObstacleType)
        {
            map = GameMap;
            obstacleType = ObstacleType;
            mapWidth = map.GetLength(0);
            mapHeight= map.GetLength(1);
        }

        /// <summary>
        /// 寻找路径
        /// </summary>
        /// <param name="startNode">起始节点</param>
        /// <param name="endNode">终止节点</param>
        /// <param name="passNodes">路径节点数组</param>
        /// <returns>寻路结果</returns>
        public bool FindPath(AStarNode startNode, AStarNode endNode, ref List<AStarNode> passNodes)
        {
            passNodes.Clear();
            if (startNode.EqualOther(endNode))
            {
                return false;
            }
            
            OpenNodeList.Clear();
            CloseNodeList.Clear();

            startNode.G = 0;
            startNode.H = startNode.GetHValue(endNode);
            startNode.ParentNode = null;
            
            OpenNodeList.Add(startNode);

            AStarNode currentNode = null;

            while (OpenNodeList.Count>0)
            {
                currentNode = GetMinFNode();

                if (currentNode.EqualOther(endNode))
                {
                    while (!currentNode.EqualOther(startNode))
                    {
                        passNodes.Add(currentNode);
                        currentNode = currentNode.ParentNode;
                    }
                    passNodes.Add(startNode);
                    passNodes.Reverse();
                    OpenNodeList.Clear();
                    CloseNodeList.Clear();
                    return true;
                }
                
                OpenNodeList.Remove(currentNode);
                CloseNodeList.Add(currentNode);
                
                //检查邻居节点
                UpdateNeighborNodes(currentNode,endNode);
            }
            
            passNodes.Clear();
            OpenNodeList.Clear();
            CloseNodeList.Clear();
            return false;
        }

        /// <summary>
        /// 更新当前节点周围邻居的状态
        /// </summary>
        /// <param name="currentNode">当前节点</param>
        /// <param name="endNode">目标节点</param>
        public void UpdateNeighborNodes(AStarNode currentNode,AStarNode endNode)
        {

            List<AStarNode> neighborNodes = currentNode.NeighborNotes;

            for (int i = 0; i < neighborNodes.Count; i++)
            {
                int x = neighborNodes[i].Point.X;
                int y = neighborNodes[i].Point.Y;

                if (x >= 0 && x < mapWidth && y >= 0 && y < mapHeight)
                {
                    if (map[x, y] != obstacleType && !InNodeList(neighborNodes[i], CloseNodeList))
                    {
                        neighborNodes[i].NodeType = map[x, y];
                        if (InNodeList(neighborNodes[i], OpenNodeList))
                        {
                            AStarNode node = OpenNodeList.FirstOrDefault(n => n.Point == neighborNodes[i].Point);
                            int GValue=currentNode.GetGValue(neighborNodes[i]) + currentNode.G;
                            if (GValue < node.G)
                            {
                                node.ParentNode = currentNode;
                                node.G = GValue;
                            }
                        }
                        else
                        {
                            neighborNodes[i].ParentNode = currentNode;
                            int GValue = currentNode.GetGValue(neighborNodes[i]) + currentNode.G;
                            neighborNodes[i].G = GValue;
                            neighborNodes[i].H = neighborNodes[i].GetHValue(endNode);
                            OpenNodeList.Add(neighborNodes[i]);
                        }
                    }
                    
                }
            }
        }
        
        /// <summary>
        /// 获取开节点列表中F值最小的节点
        /// </summary>
        /// <returns>F值最小的节点</returns>
        public AStarNode GetMinFNode()
        {
            AStarNode result = null;
            int fValue = int.MaxValue;
            for (int i = 0; i < OpenNodeList.Count; i++)
            {
                if (OpenNodeList[i].F < fValue)
                {
                    fValue = OpenNodeList[i].F;
                    result = OpenNodeList[i];
                }
            }

            return result;
        }

        /// <summary>
        /// 判断节点是否在列表中
        /// </summary>
        /// <param name="node">带判断节点</param>
        /// <param name="nodeList">列表</param>
        /// <returns>判断结果</returns>
        public bool InNodeList(AStarNode node,List<AStarNode> nodeList)
        {
            return nodeList.FirstOrDefault(temp => temp.Point == node.Point) != null;
        }

    }
    
}

