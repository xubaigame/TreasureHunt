// ****************************************************
//     文件：CantCoverElement.cs
//     作者：积极向上小木木
//     邮箱: positivemumu@126.com
//     日期：2021/1/21 9:16:26
//     功能：不可翻开元素积累
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CantCoverElement : BaseElement
{
    public override void Awake()
    {
        base.Awake();
        elementType = ElementTypes.CantCovered;
        elementState = ElementStates.Uncovered;
    }
}
