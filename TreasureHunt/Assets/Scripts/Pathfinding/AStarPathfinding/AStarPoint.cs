// ****************************************************
//     文件：AStarPoint.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/13 22:46:17
//     功能：AStar路径坐标点
// *****************************************************

using UnityEngine;

namespace AStarPathfinding
{
    public class AStarPoint
    {
        private int x;
        private int y;
        
        public int X
        {
            get => x;
            set => x = value;
        }

        public int Y
        {
            get => y;
            set => y = value;
        }

        public AStarPoint(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(AStarPoint a, AStarPoint b)
        {
            return a.X == b.X && a.Y == b.Y;
        }
        
        public static bool operator !=(AStarPoint a, AStarPoint b)
        {
            return a.X != b.X || a.Y != b.Y;
        }
    }
}

