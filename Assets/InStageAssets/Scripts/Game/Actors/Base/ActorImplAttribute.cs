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
        public string ActorID { get; }
        public string PrefabPath { get; }

        /// <param name="actorID">유니크한 이름 지정 </param>
        /// <param name="prefabRelativePath">Constants.ACTOR_RELATIVE_PATH 하위 path 지정</param> 
        public ActorImplAttribute(string actorID, string prefabRelativePath)
        {
            ActorID = actorID;
            PrefabPath = $"{Constants.ACTOR_RELATIVE_PATH}/prefabRelativePath";
        }
    }
}