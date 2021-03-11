// ****************************************************
//     文件：UnityThreadDetect.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/11 9:49:59
//     功能：死循环监测类
// *****************************************************

using System;
using UnityEngine;
using System.Threading;

public static class UnityThreadDetect
{
    public static Thread _MainThread = System.Threading.Thread.CurrentThread;//获取unity线程
    private static int check_interval = 3000;//检测间隔
}