// ****************************************************
//     文件：EnemyElement.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/1/21 9:19:47
//     功能：敌人元素类
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyElement : CantCoverElement
{
   public override void Awake()
   {
      base.Awake();
      elementContent = ElementContents.Enemy;
      ClearShadow();
      LoadSprite(MapManager.Instance.mapData.Enemys[Random.Range(0,MapManager.Instance.mapData.Enemys.Length)]);
   }

   public override void OnLeftMouseButtonDown()
   {
      if (Vector3.Distance(transform.position, PlayerManager.Instance.transform.position) < 1.5f)
      {
         switch (GameDataManager.Instance.weaponType)
         {
            case WeaponTypes.None:
               base.OnLeftMouseButtonDown();
               break;
            case WeaponTypes.Arrow:
               AudioManager.Instance.PlayEffect(Consts.enemy);
               GameDataManager.Instance.ChangeWeapon(WeaponTypes.Arrow, -1);
               MapManager.Instance.ChangeToNumberElement(this, true);
               break;
            case WeaponTypes.Sword:
               AudioManager.Instance.PlayEffect(Consts.enemy);
               MapManager.Instance.ChangeToNumberElement(this, true);
               break;
         }
         
      }
      else
      {
         base.OnLeftMouseButtonDown();
      }
   }
}
