// ****************************************************
//     文件：AutoDestory.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/26 12:36:50
//     功能：
// *****************************************************

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestory : MonoBehaviour
{
    public float destoryTime;
    void Start()
    {
        Destroy(this.gameObject,destoryTime);
    }
}
