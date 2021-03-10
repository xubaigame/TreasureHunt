// ****************************************************
//     文件：LevelData.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2020/12/28 22:51:0
//     功能：关卡数据
// *****************************************************

using UnityEngine;

[CreateAssetMenu(menuName = "GameDataMenu/Create LevelData ")]
public class LevelData : ScriptableObject
{
    [Header("关卡数据")]
    public float MinTrapProbability;
    public float MaxTrapProbability;
    public float UncoveredProbability;
    public int StandAreaWidth;
    public int ObstacleWidth;
}
