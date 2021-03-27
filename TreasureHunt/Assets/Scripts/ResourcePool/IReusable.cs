// ****************************************************
//     文件：IReusable.cs
//     作者：积极向上小木木
//     邮箱：positivemumu@126.com
//     日期：2021/3/26 22:26:21
//     功能：资源池回收方法接口
// *****************************************************

namespace MFramework.ResourcePool
{
    public interface IReusable
    {
        void OnSpawn();

        void OnUnspawn();
    }
}