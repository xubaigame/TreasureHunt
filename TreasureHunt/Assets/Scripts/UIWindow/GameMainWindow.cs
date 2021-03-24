// ****************************************************
//     文件：GameMainWindow.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/24 19:11:9
//     功能：游戏主界面
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameMainWindow : MonoBehaviour
{

    private bool isHide=false;
    
    public Image armorIcon;
    public Image keyIcon;
    public Image arrowBg;
    public Image arrowIcon;
    public Image swordIcon;
    public Image hoeBag;
    public Image hoeIcon;
    public Image tntBag;
    public Image tntIcon;
    public Image mapBag;
    public Image mapIcon;
    public Image grassIcon;
    public Text levelText;
    public Text hpText;
    public Text armorText;
    public Text keyText;
    public Text weaponText;
    public Text hoeText;
    public Text tntText;
    public Text mapText;
    public Text goldText;

    private void Start()
    {
        UpdateUI();
    }

    public void OnLevelButtonDown()
    {
        if (!isHide)
        {
            GetComponent<RectTransform>().DOAnchorPosY(-7, 0.5f);
            isHide = true;
        }
        else
        {
            GetComponent<RectTransform>().DOAnchorPosY(67, 0.5f);
            isHide = false;
        }
    }

    public void UpdateUI()
    {
        levelText.text = "Level " + GameDataManager.Instance.gameData.Level;
        hpText.text = GameDataManager.Instance.gameData.Hp.ToString();
        if (GameDataManager.Instance.gameData.Armor==0)
        {
            armorIcon.gameObject.SetActive(false);
            armorText.gameObject.SetActive(false);
        }
        else
        {
            armorIcon.gameObject.SetActive(true);
            armorText.gameObject.SetActive(true);
        }
        
        if (GameDataManager.Instance.gameData.Key==0)
        {
            keyIcon.gameObject.SetActive(false);
            keyText.gameObject.SetActive(false);
        }
        else
        {
            keyIcon.gameObject.SetActive(true);
            keyText.gameObject.SetActive(true);
        }

        switch (GameDataManager.Instance.weaponType)
        {
            case WeaponTypes.None:
                arrowBg.gameObject.SetActive(true);
                arrowIcon.gameObject.SetActive(false);
                swordIcon.gameObject.SetActive(false);
                weaponText.gameObject.SetActive(false);
                break;
            case WeaponTypes.Arrow:
                arrowBg.gameObject.SetActive(false);
                arrowIcon.gameObject.SetActive(true);
                swordIcon.gameObject.SetActive(false);
                weaponText.gameObject.SetActive(false);
                weaponText.text = GameDataManager.Instance._arrow.ToString();
                break;
            case WeaponTypes.Sword:
                arrowBg.gameObject.SetActive(false);
                arrowIcon.gameObject.SetActive(false);
                swordIcon.gameObject.SetActive(true);
                weaponText.gameObject.SetActive(false);
                break;
        }

        if (GameDataManager.Instance.gameData.Hoe == 0)
        {
            hoeBag.gameObject.SetActive(false);
            hoeIcon.gameObject.SetActive(false);
            hoeText.gameObject.SetActive(false);
        }
        else
        {
            hoeBag.gameObject.SetActive(true);
            hoeIcon.gameObject.SetActive(true);
            hoeText.gameObject.SetActive(true);
            hoeText.text = GameDataManager.Instance.gameData.Hoe.ToString();
        }
        
        if (GameDataManager.Instance.gameData.Tnt == 0)
        {
            tntBag.gameObject.SetActive(false);
            tntIcon.gameObject.SetActive(false);
            tntText.gameObject.SetActive(false);
        }
        else
        {
            tntBag.gameObject.SetActive(true);
            tntIcon.gameObject.SetActive(true);
            tntText.gameObject.SetActive(true);
            tntText.text = GameDataManager.Instance.gameData.Hoe.ToString();
        }
        
        if (GameDataManager.Instance.gameData.Map == 0)
        {
            mapBag.gameObject.SetActive(false);
            mapIcon.gameObject.SetActive(false);
            mapText.gameObject.SetActive(false);
        }
        else
        {
            mapBag.gameObject.SetActive(true);
            mapIcon.gameObject.SetActive(true);
            mapText.gameObject.SetActive(true);
            mapText.text = GameDataManager.Instance.gameData.Hoe.ToString();
        }
        
        grassIcon.gameObject.SetActive(GameDataManager.Instance.gameData.Grass);

        goldText.text = GameDataManager.Instance.gameData.Gold.ToString();
    }
   

}
