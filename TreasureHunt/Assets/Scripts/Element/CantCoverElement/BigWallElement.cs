// ****************************************************
//     文件：BigWallElement.cs
//     作者：积极向上小木木
//     邮箱: positivemumu@126.com
//     日期：2021/1/21 9:19:10
//     功能：不可摧毁的墙元素类
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigWallElement : CantCoverElement
{
    public override void Awake()
    {
        base.Awake();
        elementContent = ElementContents.BigWall;
        LoadSprite(MapManager.Instance.mapData.BigWalls[Random.Range(0,MapManager.Instance.mapData.BigWalls.Length)]);
    }
}
