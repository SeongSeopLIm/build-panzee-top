using UnityEngine;

namespace WAK
{
    /// <summary>
    /// ��ġ���� ���� ������� ������Ʈ 
    /// </summary>
    public class Actor : MonoBehaviour
    {
        protected ActorImpl impl;
        public ActorImpl Impl => impl;
         

        public virtual void OnCreated(ActorImpl impl)
        {
            this.impl = impl;
            impl?.OnCreated();
        }
         
        public virtual void OnSpawn()
        {
            impl?.OnSpawn(gameObject.GetInstanceID());
        }


        public virtual void OnDespawn()
        {
            impl?.OnDespawn();
        }
    }
}