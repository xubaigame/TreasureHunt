// ****************************************************
//     文件：SmallWallElement.cs
//     作者：积极向上小木木
//     邮箱: positivemumu@126.com
//     日期：2021/1/21 9:19:25
//     功能：可摧毁的墙元素类
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallWallElement : CantCoverElement
{
    public override void Awake()
    {
        base.Awake();
        elementContent = ElementContents.SmallWall;
        ClearShadow();
        LoadSprite(MapManager.Instance.mapData.SmallWalls[Random.Range(0,MapManager.Instance.mapData.SmallWalls.Length)]);
    }
}
