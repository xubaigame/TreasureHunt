// ****************************************************
//     文件：DoubleCoverElement.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/1/20 15:1:15
//     功能：双翻元素基类
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCoverElement : SingleCoverElement
{
    public bool isHide = true;
    public override void Awake()
    {
        base.Awake();
        elementType = ElementTypes.DoubleCovered;
        if (Random.value < MapManager.Instance.LevelData.UncoveredProbability)
        {
            UncovredElementFirst();
        }
    }
    public override void OnPlayerStand()
    {
        if(elementState==ElementStates.Covered)
        {
            if (isHide)
            {
                UncovredElementFirst();
            }
            else
            {
                OnCovered();
            }
        } 
    }
    public override void OnRightMouseButtonDown()
    {
        switch (elementState)
        {
            case ElementStates.Covered:
                if (isHide)
                {
                    AddFlag();
                }
                break;
            case ElementStates.Uncovered:
                break;
            case ElementStates.Marked:
                if (isHide)
                {
                    RemoveFlag();
                }
                break;
            default:
                break;
        }
    }

    public override void UncovredElementFirst()
    {
        isHide = false;
        ClearShadow();
        ChangeSprite();
    }

    public override void OnCovered()
    {
        elementState = ElementStates.Uncovered;
        HandlePlayer();
        MapManager.Instance.ChangeToNumberElement(this,false);
    }

    public virtual void ChangeSprite()
    {
        
    }

    public virtual void HandlePlayer()
    {
        
    }
}
