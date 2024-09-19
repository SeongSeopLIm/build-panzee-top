namespace WAK
{

    [ActorImpl(poolType: "Actor", prefabRelativePath: "/ActorBase.prefab")]
    public class ActorImpl
    {
        /// <summary>
        /// GameObject.InstanceID
        /// </summary>
        public int ObjectID { get; private set; }

        /// <summary>
        /// -1 is not setted
        /// </summary>
        protected int dataKey { get; private set; } = -1;

        public virtual void OnCreated()
        {

        }

        public virtual void OnSpawn(int objectID )
        {
            ObjectID = objectID;
        } 

        public virtual void OnDespawn()
        {

        } 

        /// <param name="dataKey">������ Ǯ���� �������µ� ���Ǵ� Ű 0 �̻����� ����</param>
        public virtual void Set(ObjectManager.ObjectParams objectParams)
        {// DI ���Ʊ���  ����
            this.dataKey = objectParams.dataKey;
        }
    }
}