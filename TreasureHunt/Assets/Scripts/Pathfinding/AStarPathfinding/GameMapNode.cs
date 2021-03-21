// ****************************************************
//     文件：GameMapNode.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/21 15:10:15
//     功能：
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AStarPathfinding;

public class GameMapNode : AStarNode
{
    public GameMapNode(AStarPoint point, int nodeType) : base(point, nodeType)
    {
    }

    public GameMapNode(int x, int y, int nodeType) : base(x, y, nodeType)
    {
    }

    public override List<AStarNode> NeighborNotes
    {
        get
        {
            neighborNodes.Clear();
            neighborNodes.Add(new GameMapNode(Point.X-1,Point.Y,0));
            neighborNodes.Add(new GameMapNode(Point.X-1,Point.Y-1,0));
            neighborNodes.Add(new GameMapNode(Point.X-1,Point.Y+1,0));
            neighborNodes.Add(new GameMapNode(Point.X+1,Point.Y,0));
            neighborNodes.Add(new GameMapNode(Point.X+1,Point.Y-1,0));
            neighborNodes.Add(new GameMapNode(Point.X+1,Point.Y+1,0));
            neighborNodes.Add(new GameMapNode(Point.X,Point.Y-1,0));
            neighborNodes.Add(new GameMapNode(Point.X,Point.Y+1,0));
            return neighborNodes;
        }
    }

    public override int GetGValue(AStarNode node)
    {
        if (node.Point == Point)
        {
            return 0;
        }
        if(node.NodeType==3)
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
    
    public override int GetHValue(AStarNode node)
    {
        int temp=base.GetHValue(node);
        if (NodeType == 1)
            temp += 1000;
        else if(NodeType==3)
        {
            temp = 0;
        }
        
        return temp;
    }
}
