// ****************************************************
//     文件：ToolElement.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
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
        switch (toolType)
        {
            case ToolTypes.Hp:
                GameDataManager.Instance.ChangeHp(1);
                break;
            case ToolTypes.Armor:
                GameDataManager.Instance.ChangeArmor(1);
                break;
            case ToolTypes.Key:
                GameDataManager.Instance.ChangeKey(1);
                break;
            case ToolTypes.Arrow:
                GameDataManager.Instance.ChangeWeapon(WeaponTypes.Arrow,1);
                break;
            case ToolTypes.Sword:
                GameDataManager.Instance.ChangeWeapon(WeaponTypes.Sword, 0);
                break;
            case ToolTypes.Hoe:
                GameDataManager.Instance.ChangeHoe(1);
                break;
            case ToolTypes.Tnt:
                GameDataManager.Instance.ChangeTnt(1);
                break;
            case ToolTypes.Map:
                GameDataManager.Instance.ChangeMap(1);
                break;
            case ToolTypes.Grass:
                GameDataManager.Instance.ChangeGrass(true);
                break;
        }
    }
}
