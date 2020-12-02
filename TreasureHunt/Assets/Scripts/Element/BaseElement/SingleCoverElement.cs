/****************************************************
    文件：SingleCoverElement.cs
	作者：积极向上小木木
    邮箱: positivemumu@126.com
    日期：2020/11/25 22:30:52
	功能：单翻元素基类
*****************************************************/

using DG.Tweening;
using UnityEngine;

public class SingleCoverElement : BaseElement 
{
    public override void Awake()
    {
        base.Awake();
        elementType = ElementTypes.SingleCovered;
        elementState = ElementStates.Covered;
        LoadSprite(MapManager.Instance.Tiles[Random.Range(0, MapManager.Instance.Tiles.Length)]);
    }

    public override void OnPlayerStand()
    {
        if(elementState==ElementStates.Covered)
        {
            UncovredElementFirst();
            OnCovered();
        }
    }

    /// <summary>
    /// 第一次翻开元素
    /// </summary>
    public virtual void UncovredElementFirst() { }

    /// <summary>
    /// 翻开元素后操作
    /// </summary>
    public virtual void OnCovered() { }

    public override void OnRightMouseButtonDown()
    {
        switch (elementState)
        {
            case ElementStates.Covered:
                AddFlag();
                break;
            case ElementStates.Uncovered:
                break;
            case ElementStates.Marked:
                RemoveFlag();
                break;
            default:
                break;
        }
    }

    public void AddFlag()
    {
        elementState = ElementStates.Marked;
        GameObject flag = Instantiate(MapManager.Instance.FlagElement, transform);
        flag.name = "FlagElement";
        flag.transform.DOLocalMoveY(0, 0.1f);
        Instantiate(MapManager.Instance.FlagEffect, transform);
    }

    public void RemoveFlag()
    {
        Transform FlagElement = transform.Find("FlagElement");
        if(FlagElement!=null)
        {
            elementState = ElementStates.Covered;
            FlagElement.DOLocalMoveY(0.15f, 0.1f).onComplete += () =>
            {
                Destroy(FlagElement.gameObject);
            };
        }
    }
}