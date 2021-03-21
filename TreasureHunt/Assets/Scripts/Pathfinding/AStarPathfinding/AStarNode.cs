// ****************************************************
//     文件：AStarNode.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/13 22:52:58
//     功能：AStar寻路节点
// *****************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStarPathfinding
{
    public class AStarNode
    {
        public AStarPoint Point;
        public AStarNode ParentNode;
        protected List<AStarNode> neighborNodes;

        private int g;
        private int h;
        private int nodeType = 0;

        /// <summary>
        /// 节点类型
        /// </summary>
        public int NodeType
        {
            get => nodeType;
            set => nodeType = value;
        }

        /// <summary>
        /// 当前点到终点的最终得分
        /// F=G+H
        /// </summary>
        public virtual int F
        {
            get => g + h;
        }

        /// <summary>
        /// 起点到当前点的得分
        /// </summary>
        public int G
        {
            get => g;
            set => g = value;
        }

        /// <summary>
        /// 当前点到终点的最终估计得分
        /// </summary>
        public int H
        {
            get => h;
            set => h = value;
        }

        public virtual List<AStarNode> NeighborNotes
        {
            get
            {
                neighborNodes.Clear();
                neighborNodes.Add(new AStarNode(Point.X-1,Point.Y,0));
                neighborNodes.Add(new AStarNode(Point.X-1,Point.Y-1,0));
                neighborNodes.Add(new AStarNode(Point.X-1,Point.Y+1,0));
                neighborNodes.Add(new AStarNode(Point.X+1,Point.Y,0));
                neighborNodes.Add(new AStarNode(Point.X+1,Point.Y-1,0));
                neighborNodes.Add(new AStarNode(Point.X+1,Point.Y+1,0));
                neighborNodes.Add(new AStarNode(Point.X,Point.Y-1,0));
                neighborNodes.Add(new AStarNode(Point.X,Point.Y+1,0));
                return neighborNodes;
            }
        }
        

        public virtual int GetGValue(AStarNode node)
        {
            if (node.Point == Point)
            {
                return 0;
            }
            if (node.Point.X == Point.X || node.Point.Y == Point.Y)
            {
                return 10;
            }
            else
            {
                return 14;
            }
        }

        public virtual int GetHValue(AStarNode node)
        {
            return GetHValueByEuclidianDistance(node);
        }

        /// <summary>
        /// 曼哈顿距离计算H值
        /// </summary>
        /// <param name="node">目标节点</param>
        /// <returns>H值</returns>
        public int GetHValueByManhattanDistance(AStarNode node)
        {
            return Math.Abs(Point.X - node.Point.X) + Math.Abs(Point.Y - node.Point.Y);
        }
        
        /// <summary>
        /// 欧氏距离计算H值
        /// </summary>
        /// <param name="node">目标节点</param>
        /// <returns>H值</returns>
        public int GetHValueByEuclidianDistance(AStarNode node)
        {
            return (int)Math.Sqrt(Math.Pow(Point.X - node.Point.X, 2)+ Math.Pow(Point.Y - node.Point.Y, 2));
        }
        
        /// <summary>
        /// 欧氏距离的平方计算H值
        /// </summary>
        /// <param name="node">目标节点</param>
        /// <returns>H值</returns>
        public int GetHValueByPowEuclidianDistance(AStarNode node)
        {
            return (int)(Math.Pow(Point.X - node.Point.X, 2)+ Math.Pow(Point.Y - node.Point.Y, 2));
        }

        /// <summary>
        /// 判断两个节点是否相同
        /// </summary>
        /// <param name="node">目标节点</param>
        /// <returns>比较结果</returns>
        public virtual bool EqualOther(AStarNode node)
        {
            if (node == null)
                return false;
            return node.Point == Point;
        }
        
        public AStarNode(AStarPoint point,int nodeType)
        {
            Point = point;
            NodeType = nodeType;
            neighborNodes = new List<AStarNode>();
        }

        public AStarNode(int x, int y,int nodeType)
        {
            Point = new AStarPoint(x, y);
            NodeType = nodeType;
            neighborNodes = new List<AStarNode>();
        }
    }
}

