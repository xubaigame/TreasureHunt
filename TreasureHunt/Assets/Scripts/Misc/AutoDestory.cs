// ****************************************************
//     文件：AutoDestory.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/26 12:36:50
//     功能：
// *****************************************************

using System;
using System.Collections;
using System.Collections.Generic;
using MFramework.ResourcePool;
using UnityEngine;

public class AutoDestory : MonoBehaviour
{
    public float unspawnTime;

    private void OnEnable()
    {
        Invoke("RecycleGameObject",unspawnTime);
        //Destroy(this.gameObject,destoryTime);
    }

    public void RecycleGameObject()
    {
        ObjectPool.Instance.Unspawn(this.gameObject);
    }
    
}
