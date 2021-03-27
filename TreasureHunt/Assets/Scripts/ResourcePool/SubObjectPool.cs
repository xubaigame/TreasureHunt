// ****************************************************
//     文件：SubObjectPool.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/26 23:36:3
//     功能：子资源池类
// *****************************************************

using System.Collections.Generic;
using UnityEngine;

namespace MFramework.ResourcePool
{
    public class SubObjectPool
    {
        private GameObject m_prefab;

        private List<GameObject> m_objects = new List<GameObject>();

        public string Name
        {
            get => m_prefab.name;
        }

        public SubObjectPool(GameObject prefab)
        {
            this.m_prefab = prefab;
        }

        public GameObject Spawn()
        {
            GameObject go = null;
            foreach (GameObject obj in m_objects)
            {
                if (!obj.activeSelf)
                {
                    go = obj;
                    break;
                }
            }

            if (go == null)
            {
                go = GameObject.Instantiate<GameObject>(m_prefab);
                m_objects.Add(go);
            }

            go.SetActive(true);
            go.SendMessage("OnSpawn",SendMessageOptions.DontRequireReceiver);
            return go;
        }
        
        public void Unspawn(GameObject go)
        {
            if (Contains(go))
            {
                go.SendMessage("OnUnspawn",SendMessageOptions.DontRequireReceiver);
                go.SetActive(false);
            }
        }

        public void UnspawnAll()
        {
            foreach (GameObject obj in m_objects)
            {
                if(obj.activeSelf)
                    Unspawn(obj);
            }
        }

        public bool Contains(GameObject go)
        {
            return m_objects.Contains(go);
        }
    }
}

