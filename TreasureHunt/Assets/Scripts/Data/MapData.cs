/****************************************************
    文件：MapData.cs
	作者：积极向上小木木
    邮箱：positivemumu@126.com
    日期：2020/12/6 22:2:19
	功能：地图数据
*****************************************************/

using UnityEngine;

[CreateAssetMenu(menuName = "GameDataMenu/Create MapData ")]
public class MapData : ScriptableObject 
{
    [Header("地图数据")]
    //图片数据
    public Sprite[] Tiles;
    public Sprite[] Numbers;
    public Sprite[] Traps;
    public Sprite[] Tools;
    public Sprite[] Golds;
    public Sprite[] BigWalls;
    public Sprite[] SmallWalls;
    public Sprite[] Enemys;
    public Sprite Door;
    public Sprite Exit;

    //元素预制体
    public GameObject[] MapBorder;
    public GameObject MapBackGround;
    public GameObject FlagElement;
    public GameObject BaseElement;
    public GameObject ErrorElement;
}