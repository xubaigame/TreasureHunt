// ****************************************************
//     文件：ExitElement.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/10 15:48:39
//     功能：出口元素类
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitElement : CantCoverElement
{
    public override void Awake()
    {
        base.Awake();
        elementContent = ElementContents.Exit;
        LoadSprite(MapManager.Instance.mapData.Exit);
    }
}
