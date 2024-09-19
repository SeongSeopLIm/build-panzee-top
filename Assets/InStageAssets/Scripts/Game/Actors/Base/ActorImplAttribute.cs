// ActorAttribute.cs
using System;

namespace WAK
{

    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ActorImplAttribute : Attribute
    {
        /// <summary>
        /// ObjectPool 그룹별 유니크 이름.
        /// </summary>
        public string PoolID { get; }
        public string PrefabPath { get; }

        /// <param name="poolType">유니크한 이름 지정 Pool 관리에 사용됨.</param>
        /// <param name="prefabRelativePath">Constants.ACTOR_RELATIVE_PATH 하위 path 지정</param> 
        public ActorImplAttribute(string poolType, string prefabRelativePath)
        {
            PoolID = poolType;
            PrefabPath = $"{Constants.ACTOR_RELATIVE_PATH}/prefabRelativePath";
        }
    }
}