// ****************************************************
//     文件：ReusableObject.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/26 22:31:2
//     功能：资源池元素基类
// *****************************************************

using UnityEngine;

namespace MFramework.ResourcePool
{
    public abstract class ReusableObject : MonoBehaviour,IReusable
    {

        public abstract void OnSpawn();

        public abstract void OnUnspawn();
    }
}

