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
        public string ActorID { get; }
        public string PrefabPath { get; }

        /// <param name="actorID">����ũ�� �̸� ���� </param>
        /// <param name="prefabRelativePath">Constants.ACTOR_RELATIVE_PATH ���� path ����</param> 
        public ActorImplAttribute(string actorID, string prefabRelativePath)
        {
            ActorID = actorID;
            PrefabPath = $"{Constants.ACTOR_RELATIVE_PATH}/prefabRelativePath";
        }
    }
}