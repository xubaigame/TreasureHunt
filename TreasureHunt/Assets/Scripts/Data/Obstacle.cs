// ****************************************************
//     文件：Obstacle.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/10 15:48:39
//     功能：障碍物类
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle
{
    public ObstacleTypes type;
    public ObstacleDoorTypes doorType;
    public Vector2 dp;

    public int sx, sy;
    public int ex, ey;
    
    public int goldNums;

    public Vector2 gp;

    public int tx,ty;


    public void WithOneWallArea()
    {

    }

    public void WithTwoWallArea()
    {

    }
}
