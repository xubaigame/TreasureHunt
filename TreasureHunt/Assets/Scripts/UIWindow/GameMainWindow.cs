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

    public GameObject WinWindow;
    public GameObject LoseWindow;
    
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

    
    private bool isHide=false;
    private void Awake()
    {
        GameDataManager.Instance.LevelChange+= UpdateLevel;
        GameDataManager.Instance.HPChange += UpdateHp;
        GameDataManager.Instance.ArmorChange += UpdateArmor;
        GameDataManager.Instance.KeyChange += UpdateKey;
        GameDataManager.Instance.WeaponChange += UpdateWeapon;
        GameDataManager.Instance.HoeChange += UpdateHoe;
        GameDataManager.Instance.TntChange += UpdateTnt;
        GameDataManager.Instance.MapChange += UpdateMap;
        GameDataManager.Instance.GrassChange += UpdateGrass;
        GameDataManager.Instance.GoldChange += UpdateGold;

    }

    private void Start()
    {
        UpdateUI();
    }

    private void OnDestroy()
    {
        GameDataManager.Instance.LevelChange-= UpdateLevel;
        GameDataManager.Instance.HPChange -= UpdateHp;
        GameDataManager.Instance.ArmorChange -= UpdateArmor;
        GameDataManager.Instance.KeyChange -= UpdateKey;
        GameDataManager.Instance.WeaponChange -= UpdateWeapon;
        GameDataManager.Instance.HoeChange -= UpdateHoe;
        GameDataManager.Instance.TntChange -= UpdateTnt;
        GameDataManager.Instance.MapChange -= UpdateMap;
        GameDataManager.Instance.GrassChange -= UpdateGrass;
        GameDataManager.Instance.GoldChange -= UpdateGold;

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

    public void ChangeHoeSelectState(bool state)
    {
        PlayerManager.Instance.hoeSelect.SetActive(state);
    }
    
    public void ChangeTntSelectState(bool state)
    {
        PlayerManager.Instance.tntSelect.SetActive(state);
    }
    
    public void ChangeMapSelectState(bool state)
    {
        PlayerManager.Instance.mapSelect.SetActive(state);
    }
    public void UpdateUI()
    {
        UpdateLevel(GameDataManager.Instance.gameData.Level);
        UpdateHp(GameDataManager.Instance.gameData.Hp);
        UpdateArmor(GameDataManager.Instance.gameData.Armor);
        UpdateKey(GameDataManager.Instance.gameData.Key);
        UpdateWeapon(GameDataManager.Instance.weaponType,GameDataManager.Instance._arrow);
        UpdateHoe(GameDataManager.Instance.gameData.Hoe);
        UpdateTnt(GameDataManager.Instance.gameData.Tnt);
        UpdateMap(GameDataManager.Instance.gameData.Map);
        UpdateGrass(GameDataManager.Instance.gameData.Grass);
        UpdateGold(GameDataManager.Instance.gameData.Gold);
    }

    public void UpdateLevel(int level)
    {
        levelText.text = "Level "+level;
        levelText.rectTransform.DOShakeScale(0.5f).onComplete += () =>
        {
            levelText.rectTransform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        };
    }

    public void UpdateHp(int hp)
    {
        hpText.text = hp.ToString();
        if (hp <= 0)
        {
            MapManager.Instance.ShowAllTrap();
            PlayerManager.Instance.ShowDieAnimation();
            MapManager.Instance.transform.position = new Vector3(0, 0, -1);
            LoseWindow.SetActive(true);
            GameDataManager.Instance.NewGame();
        }
    }

    public void UpdateArmor(int armorNumber)
    {
        if (armorNumber==0)
        {
            armorIcon.gameObject.SetActive(false);
            armorText.gameObject.SetActive(false);
        }
        else
        {
            armorIcon.gameObject.SetActive(true);
            armorText.gameObject.SetActive(true);
            armorText.text = armorNumber.ToString();
            armorIcon.rectTransform.DOShakeScale(0.5f).onComplete += () =>
            {
                armorIcon.rectTransform.localScale = Vector3.one;
            };
            armorText.rectTransform.DOShakeScale(0.5f).onComplete += () =>
            {
                armorText.rectTransform.localScale = Vector3.one;
            };
        }
    }

    public void UpdateKey(int keyNumber)
    {
        if (keyNumber==0)
        {
            keyIcon.gameObject.SetActive(false);
            keyText.gameObject.SetActive(false);
        }
        else
        {
            keyIcon.gameObject.SetActive(true);
            keyText.gameObject.SetActive(true);
            keyText.text = keyNumber.ToString();
            keyIcon.rectTransform.DOShakeScale(0.5f).onComplete += () =>
            {
                keyIcon.rectTransform.localScale = Vector3.one;
            };
            keyText.rectTransform.DOShakeScale(0.5f).onComplete += () =>
            {
                keyText.rectTransform.localScale = Vector3.one;
            };
        }
    }

    public void UpdateWeapon(WeaponTypes weaponType,int arrowNumber)
    {
        switch (weaponType)
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
                weaponText.gameObject.SetActive(true);
                weaponText.text = arrowNumber.ToString();
                arrowIcon.rectTransform.DOShakeScale(0.5f).onComplete += () =>
                {
                    arrowIcon.rectTransform.localScale = Vector3.one;
                };
                weaponText.rectTransform.DOShakeScale(0.5f).onComplete += () =>
                {
                    weaponText.rectTransform.localScale = Vector3.one;
                };
                break;
            case WeaponTypes.Sword:
                arrowBg.gameObject.SetActive(false);
                arrowIcon.gameObject.SetActive(false);
                swordIcon.gameObject.SetActive(true);
                weaponText.gameObject.SetActive(false);
                swordIcon.rectTransform.DOShakeScale(0.5f).onComplete += () =>
                {
                    swordIcon.rectTransform.localScale = Vector3.one;
                };
                break;
        }
    }

    public void UpdateHoe(int hoeNumber)
    {
        if (hoeNumber == 0)
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
            hoeText.text = hoeNumber.ToString();
            hoeIcon.rectTransform.DOShakeScale(0.5f).onComplete += () =>
            {
                hoeIcon.rectTransform.localScale = Vector3.one;
            };
            hoeText.rectTransform.DOShakeScale(0.5f).onComplete += () =>
            {
                hoeText.rectTransform.localScale = Vector3.one;
            };
        }
    }

    public void UpdateTnt(int tntNumber)
    {
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
            tntText.text = tntNumber.ToString();
            tntIcon.rectTransform.DOShakeScale(0.5f).onComplete += () =>
            {
                tntIcon.rectTransform.localScale = Vector3.one;
            };
            tntText.rectTransform.DOShakeScale(0.5f).onComplete += () =>
            {
                tntText.rectTransform.localScale = Vector3.one;
            };
        }
    }

    public void UpdateMap(int mapNumber)
    {
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
            mapIcon.rectTransform.DOShakeScale(0.5f).onComplete += () =>
            {
                mapIcon.rectTransform.localScale = Vector3.one;
            };
            mapText.rectTransform.DOShakeScale(0.5f).onComplete += () =>
            {
                mapText.rectTransform.localScale = Vector3.one;
            };
        }
    }

    public void UpdateGrass(bool state)
    {
        grassIcon.gameObject.SetActive(state);
        grassIcon.rectTransform.DOShakeScale(0.5f).onComplete += () =>
        {
            grassIcon.rectTransform.localScale = Vector3.one;
        };
    }

    public void UpdateGold(int goldNumber)
    {
        goldText.text = GameDataManager.Instance.gameData.Gold.ToString();
        goldText.rectTransform.DOShakeScale(0.5f).onComplete += () =>
        {
            goldText.rectTransform.localScale = Vector3.one;
        };
    }
    
    
}
