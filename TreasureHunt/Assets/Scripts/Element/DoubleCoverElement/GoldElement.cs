// ****************************************************
//     文件：GoldElement.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/1/20 15:57:16
//     功能：金币元素类
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldElement : DoubleCoverElement
{
    public GoldTypes goldType;

    public override void Awake()
    {
        base.Awake();
        elementContent = ElementContents.Gold;
    }

    public override void ChangeSprite()
    {
        Transform goldEffect = transform.Find("GoldEffect");
        if(goldEffect==null)
        {
            Instantiate(MapManager.Instance.mapData.GoldEffect,transform).name="GoldEffect";
        }
        LoadSprite(MapManager.Instance.mapData.Golds[(int)goldType]);
    }

    public void DestoryGoldEffect()
    {
        Transform goldEffect = transform.Find("GoldEffect");
        if(goldEffect!=null)
        {
            Destroy(goldEffect.gameObject);
        }
    }
    public override void HandlePlayer()
    {
        DestoryGoldEffect();
        int ratio = GameDataManager.Instance.gameData.Grass ? 2 : 1;
        switch (goldType)
        {
            case GoldTypes.One:
                GameDataManager.Instance.ChangeGold(100 * ratio);
                break;
            case GoldTypes.Two:
                GameDataManager.Instance.ChangeGold(150 * ratio);
                break;
            case GoldTypes.Three:
                GameDataManager.Instance.ChangeGold(200 * ratio);
                break;
            case GoldTypes.Four:
                GameDataManager.Instance.ChangeGold(250 * ratio);
                break;
            case GoldTypes.Five:
                GameDataManager.Instance.ChangeGold(300 * ratio);
                break;
            case GoldTypes.Six:
                GameDataManager.Instance.ChangeGold(350 * ratio);
                break;
            case GoldTypes.Seven:
                GameDataManager.Instance.ChangeGold(400 * ratio);
                break;
        }
    }
}
