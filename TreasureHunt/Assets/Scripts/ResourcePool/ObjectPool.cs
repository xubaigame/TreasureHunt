// ****************************************************
//     文件：ObjectPool.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/27 0:12:36
//     功能：对象池类
// *****************************************************

using System;
using System.Collections.Generic;
using UnityEngine;

namespace MFramework.ResourcePool
{
    public class ObjectPool : MonoBehaviour
    {
        private static ObjectPool instance = null;

        public static ObjectPool Instance
        {
            get
            {
                return instance;
            }
        }

        public void Awake()
        {
            instance = this;
        }

        public string ResourceDir = "";
        private Dictionary<string, SubObjectPool> m_pools = new Dictionary<string, SubObjectPool>();

        public GameObject Spawn(string name)
        {
            SubObjectPool pool = null;
            if (!m_pools.ContainsKey(name))
            {
                RegisterNewSubObjectPool(name);
            }
            pool = m_pools[name];
            return pool.Spawn();
        }

        public void Unspawn(GameObject go)
        {
            SubObjectPool pool = null;
            foreach (SubObjectPool p in m_pools.Values)
            {
                if (p.Contains(go))
                {
                    pool = p;
                    break;
                }
                
            }

            pool.Unspawn(go);

        }

        public void UnspawnAll()
        {
            foreach (SubObjectPool p in m_pools.Values)
            {
                p.UnspawnAll();
            }
        }

        private void RegisterNewSubObjectPool(string name)
        {
            string path = "";
            if (string.IsNullOrEmpty(ResourceDir))
                path = name;
            else
                path = ResourceDir +"/"+ name;
            
            GameObject prefabs = Resources.Load<GameObject>(path);

            SubObjectPool pool = new SubObjectPool(prefabs);
            m_pools.Add(name, pool);
        }
    }
}

