/****************************************************
    文件：Consts.cs
	作者：积极向上小木木
    邮箱: positivemumu@126.com
    日期：2020/11/25 16:36:8
	功能：常量类
*****************************************************/

using UnityEngine;

public class Consts 
{
    //PlayerPrefs
    public readonly static string P_IsMute = "Mute";
    public readonly static string P_IsFirstGame = "FirstGame";
    public readonly static string P_Level = "level";
    public readonly static string P_Hp = "Hp";
    public readonly static string P_Armor = "Armor";
    public readonly static string P_Key = "Key";
    public readonly static string P_Hoe = "Hoe";
    public readonly static string P_Tnt = "Tnt";
    public readonly static string P_Map = "Map";
    public readonly static string P_Gold = "Gold";

    public readonly static string GameDataName = "Configs/TreasureHuntGameData";
    public readonly static string VirtualPath = Application.persistentDataPath + "/TreasureHuntGameData.json";
    public readonly static string AssetsPath = Application.streamingAssetsPath + "/TreasureHuntGameData.json";
}