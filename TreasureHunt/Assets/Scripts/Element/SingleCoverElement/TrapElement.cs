/****************************************************
    文件：TrapElement.cs
	作者：积极向上小木木
    邮箱: positivemumu@126.com
    日期：2020/12/2 21:9:0
	功能：陷阱元素类
*****************************************************/

using UnityEngine;

public class TrapElement : SingleCoverElement 
{
    public override void Awake()
    {
        base.Awake();
        elementContent = ElementContents.Trap;
    }

    public override void UncovredElementFirst()
    {
        elementState = ElementStates.Uncovered;
        ClearShadow();
        Instantiate(MapManager.Instance.UncoveredEffect, transform);
        LoadSprite(MapManager.Instance.Traps[Random.Range(0,MapManager.Instance.Traps.Length)]);
    }

    public override void OnCovered()
    {
        //翻开所有陷阱
        MapManager.Instance.ShowAllTrap();
        //最终效果todo 受到伤害
    }
}