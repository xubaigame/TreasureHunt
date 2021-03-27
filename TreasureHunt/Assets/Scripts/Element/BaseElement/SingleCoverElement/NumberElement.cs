/****************************************************
    文件：NumberElement.cs
	作者：积极向上小木木
    邮箱：positivemumu@126.com
    日期：2020/12/2 21:8:46
	功能：数字元素类
*****************************************************/

using UnityEngine;
using MFramework.ResourcePool;

public class NumberElement : SingleCoverElement
{
    public bool needEffect = true;
    public override void Awake()
    {
        base.Awake();
        elementContent = ElementContents.Number;
    }

    public override void OnMiddleMouseButtonDown()
    {
        Vector2Int playerPosition = PlayerManager.Instance.GetPlayerPosition();
        if (playerPosition.x == PositionX && playerPosition.y == PositionY)
        {
            MapManager.Instance.UncoveredAdjacentElements(PositionX,PositionY);
        }
        
    }

    public override void UncovredElementFirst()
    {
        elementState = ElementStates.Uncovered;
        ClearShadow();
        if (needEffect)
        {
            GameObject go = ObjectPool.Instance.Spawn("UncoveredEffect");
            go.transform.parent = transform;
            go.transform.localPosition=Vector3.zero;
            //Instantiate(MapManager.Instance.mapData.UncoveredEffect, transform);
        }
        LoadSprite(MapManager.Instance.mapData.Numbers[MapManager.Instance.GetTrapCountAroundElement(PositionX, PositionY)]);
    }

    public override void OnCovered()
    {
        Vector2 temp = GameDataManager.Instance.GetMapSize();
        MapManager.Instance.FloodingElement(PositionX, PositionY, new bool[(int)temp.x,(int)temp.y]);
    }

}