/****************************************************
    文件：Consts.cs
	作者：积极向上小木木
    邮箱：positivemumu@126.com
    日期：2020/11/25 16:36:8
	功能：常量类
*****************************************************/

using UnityEngine;

public class Consts 
{
    public readonly static string GameDataName = "Configs/TreasureHuntGameData";
    public readonly static string AssetsPath = Application.persistentDataPath+"/TreasureHuntGameData.json";

    //PrefabName
    public readonly static string UncoveredEffect = "UncoveredEffect";
    public readonly static string GoldEffect = "GoldEffect";
    public readonly static string FlagEffect = "FlagEffect";
    public readonly static string DoorOpenEffect = "DoorOpenEffect";
    
    //AudioClipName
    public readonly static string windowBg = "winbg";
    public readonly static string button = "button";
    public readonly static string dig = "dig";
    public readonly static string end = "end";
    public readonly static string hoe = "hoe";
    public readonly static string hurt = "hurt";
    public readonly static string die = "lose";
    public readonly static string move = "move";
    public readonly static string door = "door";
    public readonly static string pass = "pass";
    public readonly static string enemy = "enemy";
    public readonly static string tnt = "tnt";
    public readonly static string map = "map";
    public readonly static string pick = "pick";
    public readonly static string createflag = "createflag";
    public readonly static string removeflag = "removeflag";
    public readonly static string why = "why";
    public readonly static string quickCheck = "quickcheck";

    public readonly static string BGM = "bgm";
}