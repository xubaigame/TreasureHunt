// ****************************************************
//     文件：SelectGizmos.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/26 16:37:41
//     功能：范围道具响应类
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectGizmos : MonoBehaviour
{
    public ToolTypes ToolType;

    private void OnMouseUp()
    {
        int x = (int) transform.position.x;
        int y = (int) transform.position.y;
        switch (ToolType)
        {
            case ToolTypes.Hoe:
                GameObject.Find("MainPanel").GetComponent<GameMainWindow>().hoeIcon.GetComponent<Toggle>().isOn = false;
                AudioManager.Instance.PlayEffect(Consts.hoe);
                GameDataManager.Instance.ChangeHoe(-1);
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        BaseElement temp= MapManager.Instance.GetElementByPosition(i, j);
                        if (temp != null)
                        {
                            if (temp.elementType != ElementTypes.CantCovered)
                            {
                                (temp as SingleCoverElement).UncovredElementFirst();
                            }
                            else if (temp.elementContent == ElementContents.SmallWall)
                            {
                                MapManager.Instance.ChangeToNumberElement(temp,false);
                            }
                                
                        }
                    }
                }
                break;
            case ToolTypes.Tnt:
                GameObject.Find("MainPanel").GetComponent<GameMainWindow>().tntIcon.GetComponent<Toggle>().isOn = false;
                AudioManager.Instance.PlayEffect(Consts.tnt);
                GameDataManager.Instance.ChangeTnt(-1);
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        BaseElement temp= MapManager.Instance.GetElementByPosition(i, j);
                        if (temp != null)
                        {
                            if (temp.elementState == ElementStates.Marked)
                            {
                                temp.OnRightMouseButtonDown();
                            }

                            if (temp.elementContent == ElementContents.Gold)
                            {
                                (temp as GoldElement).DestoryGoldEffect();
                            }
                            MapManager.Instance.ChangeToNumberElement(temp,false);
                        }
                    }
                }

                MapManager.Instance.UpdateNumberAfterTntToolUsed(x, y);
                break;
            case ToolTypes.Map:
                GameObject.Find("MainPanel").GetComponent<GameMainWindow>().mapIcon.GetComponent<Toggle>().isOn = false;
                AudioManager.Instance.PlayEffect(Consts.map);
                GameDataManager.Instance.ChangeMap(-1);
                for (int i = x - 3; i <= x + 3; i++)
                {
                    for (int j = y - 3; j <= y + 3; j++)
                    {
                        BaseElement temp= MapManager.Instance.GetElementByPosition(i, j);
                        if (temp != null)
                        {
                            if (temp.elementContent==ElementContents.Trap&&temp.elementState!=ElementStates.Marked)
                            {
                                temp.OnRightMouseButtonDown();
                            }
                            if (temp.elementContent!=ElementContents.Trap&&temp.elementState==ElementStates.Marked)
                            {
                                temp.OnRightMouseButtonDown();
                            }
                                
                        }
                    }
                }
                break;
        }
    }
}
