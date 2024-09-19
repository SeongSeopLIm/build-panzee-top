// ActorAttribute.cs
using System;

namespace WAK
{

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ActorImplAttribute : Attribute
    {
        /// <summary>
        /// ObjectPool �׷캰 ����ũ �̸�.
        /// </summary>
        public string PoolID { get; }
        public string PrefabPath { get; }

        /// <param name="poolType">����ũ�� �̸� ���� Pool ������ ����.</param>
        /// <param name="prefabRelativePath">Constants.ACTOR_RELATIVE_PATH ���� path ����</param> 
        public ActorImplAttribute(string poolType, string prefabRelativePath)
        {
            PoolID = poolType;
            PrefabPath = $"{Constants.ACTOR_RELATIVE_PATH}/prefabRelativePath";
        }
    }
}