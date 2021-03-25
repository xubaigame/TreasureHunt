/****************************************************
    文件：GameDataManager.cs
	作者：积极向上小木木
    邮箱：positivemumu@126.com
    日期：2020/11/25 16:34:59
	功能：游戏运行数据管理类
*****************************************************/

using UnityEngine;
using LitJson;
using System.IO;
using System;

public class GameDataManager :MonoBehaviour
{

    public GameData gameData;
    public WeaponTypes weaponType;
    public int _arrow;
    public Action<int,int> UpdateMapSize;

    //委托
    public Action<int> LevelChange;
    public Action<int> HPChange;
    public Action<int> ArmorChange;
    public Action<int> KeyChange;
    public Action<WeaponTypes,int> WeaponChange;
    public Action<int> HoeChange;
    public Action<int> TntChange;
    public Action<int> MapChange;
    public Action<int> GoldChange;
    public Action<bool> GrassChange;
    private static GameDataManager _instance;
    public static GameDataManager Instance
    {
        get => _instance;
    }
    private bool _showContinue = true;
    
    public void Awake()
    {
        _instance = this;
        LoadGameData();
        DontDestroyOnLoad(gameObject);
    }

    public void NewGame()
    {
        if(File.Exists(Consts.AssetsPath))
        {
            File.Delete(Consts.AssetsPath);
        }
        LoadGameData();
    }

    public bool IsFirstGame()
    {
        return !_showContinue;
    }

    public void ChangeLevel(int number)
    {
        gameData.Level += number;
        if (HPChange != null)
        {
            HPChange(gameData.Level);
        }
    }
    public void ChangeHp(int number)
    {
        gameData.Hp += number;
        if (HPChange != null)
        {
            HPChange(gameData.Hp);
        }
    }
    public void ChangeArmor(int number)
    {
        gameData.Armor += number;
        if (ArmorChange != null)
        {
            ArmorChange(gameData.Armor);
        }
    }
    public void ChangeKey(int number)
    {
        gameData.Key += number;
        if (KeyChange != null)
        {
            KeyChange(gameData.Key);
        }
    }

    public void ChangeWeapon(WeaponTypes weaponType, int number)
    {
        this.weaponType = weaponType;
        switch (this.weaponType)
        {
            case WeaponTypes.Arrow:
                _arrow += number;
                Debug.Log(_arrow);
                if (_arrow == 0)
                    this.weaponType = WeaponTypes.None;
                if (WeaponChange != null)
                {
                    WeaponChange(this.weaponType, _arrow);
                }
                break;
            case WeaponTypes.Sword:
                _arrow = 0;
                if (WeaponChange != null)
                {
                    WeaponChange(weaponType, 0);
                }
                break;
            case WeaponTypes.None:
                this.weaponType = WeaponTypes.None;
                _arrow = 0;
                break;
        }
    }
    public void ChangeHoe(int number)
    {
        gameData.Hoe += number;
        if (HoeChange != null)
        {
            HoeChange(gameData.Hoe);
        }
    }
    public void ChangeTnt(int number)
    {
        gameData.Tnt += number;
        if (TntChange != null)
        {
            TntChange(gameData.Tnt);
        }
    }
    public void ChangeMap(int number)
    {
        gameData.Map += number;
        if (MapChange != null)
        {
            MapChange(gameData.Map);
        }
    }
    
    public void ChangeGrass(bool state)
    {
        gameData.Grass = state;
        if (GrassChange != null)
        {
            GrassChange(gameData.Grass);
        }
    }

    public void ChangeGold(int number)
    {
        gameData.Gold += number;
        if (GoldChange != null)
        {
            GoldChange(gameData.Gold);
        }
    }
    public void SaveGameData()
    {
        string text = JsonMapper.ToJson(gameData);
        if(File.Exists(Consts.AssetsPath))
        {
            File.Delete(Consts.AssetsPath);
        }

        File.WriteAllText(Consts.AssetsPath, text);
    }

    private void LoadGameData()
    {
        string text = "";
        if (!File.Exists(Consts.AssetsPath))
        {
            TextAsset textAsset = Resources.Load<TextAsset>(Consts.GameDataName);
            File.WriteAllText(Consts.AssetsPath, textAsset.text);
            _showContinue = false;
        }
        else
        {
            _showContinue = true;
        }
        text = File.ReadAllText(Consts.AssetsPath);
        gameData = JsonMapper.ToObject<GameData>(text);
        gameData.Init();
        
        //Debug.Log(gameData.ToString());
    }

    public Vector2 GetMapSize()
    {
        return new Vector2(gameData.Width, gameData.Height);
    }

    private void OnDestroy()
    {
        SaveGameData();
    }
}