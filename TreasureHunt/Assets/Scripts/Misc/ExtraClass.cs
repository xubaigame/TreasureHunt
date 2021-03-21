// ****************************************************
//     文件：ExtraClass.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/21 21:35:4
//     功能：类拓展方法
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using AStarPathfinding;
using UnityEngine;

public static class ExtraClass
{
    public static Vector3[] ListToVector(this List<AStarNode> pathNode)
    {
        Vector3[] temp = new Vector3[pathNode.Count];

        for (int i = 0; i < pathNode.Count; i++)
        {
            temp[i] = new Vector3(pathNode[i].Point.X, pathNode[i].Point.Y, 0);
        }

        return temp;
    }

    public static Vector3Int ToVector3Int(this Vector3 pos)
    {
        int x = pos.x - Mathf.FloorToInt(pos.x) >= 0.5f ? Mathf.CeilToInt(pos.x) : Mathf.FloorToInt(pos.x);
        int y = pos.y - Mathf.FloorToInt(pos.y) >= 0.5f ? Mathf.CeilToInt(pos.y) : Mathf.FloorToInt(pos.y);
        return new Vector3Int(x, y, 0);
    }
}
