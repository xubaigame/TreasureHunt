// ****************************************************
//     文件：ToolElement.cs
//     作者：积极向上小木木
//     邮箱: positivemumu@126.com
//     日期：2021/1/20 15:32:51
//     功能：工具元素类
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolElement : DoubleCoverElement
{
    public ToolTypes toolType;
    public override void Awake()
    {
        base.Awake();
        elementContent = ElementContents.Tool;
        
    }

    public override void ChangeSprite()
    {
        LoadSprite(MapManager.Instance.mapData.Tools[(int)toolType]);
    }

    public override void HandlePlayer()
    {
        Debug.Log("Get a Tool");
    }
}
