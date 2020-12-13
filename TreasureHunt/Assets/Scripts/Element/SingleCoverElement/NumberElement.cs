/****************************************************
    文件：NumberElement.cs
	作者：积极向上小木木
    邮箱: positivemumu@126.com
    日期：2020/12/2 21:8:46
	功能：数字元素类
*****************************************************/

using UnityEngine;

public class NumberElement : SingleCoverElement 
{
    public override void Awake()
    {
        base.Awake();
        elementContent = ElementContents.Number;
    }

    public override void OnMiddleMouseButtonDown()
    {
        MapManager.Instance.UncoveredAdjacentElements(PositionX,PositionY);
    }

    public override void UncovredElementFirst()
    {
        elementState = ElementStates.Uncovered;
        ClearShadow();
        Instantiate(MapManager.Instance.UncoveredEffect, transform);
        LoadSprite(MapManager.Instance.Numbers[MapManager.Instance.GetTrapCountAroundElement(PositionX, PositionY)]);
    }

    public override void OnCovered()
    {
        Vector2 temp = GameDataManager.Instance.GetMapSize();
        MapManager.Instance.FloodingElement(PositionX, PositionY, new bool[(int)temp.x,(int)temp.y]);
    }

}