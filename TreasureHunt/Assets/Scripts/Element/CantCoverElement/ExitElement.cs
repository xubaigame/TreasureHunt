// ****************************************************
//     文件：ExitElement.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/10 15:48:39
//     功能：出口元素类
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitElement : CantCoverElement
{
    private bool pass = false;
    public override void Awake()
    {
        base.Awake();
        pass = false;
        elementContent = ElementContents.Exit;
        LoadSprite(MapManager.Instance.mapData.Exit);
    }

    public override void OnPlayerStand()
    {
        if (!pass)
        {
            PlayerManager.Instance.ShowPassAnimation();
            MapManager.Instance.transform.position = new Vector3(0, 0, -1);
            Camera.main.transform.GetChild(0).gameObject.SetActive(true);
            GameObject.Find("MainPanel").GetComponent<GameMainWindow>().ShowWinWindow();
            GameDataManager.Instance.EnterNextLevel();
            AudioManager.Instance.PlayEffect(Consts.pass);
            AudioManager.Instance.PlayEffect(Consts.windowBg);
            pass = true;
        }
    }
}
