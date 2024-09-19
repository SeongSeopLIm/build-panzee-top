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

        /// <param name="dataKey">데이터 풀에서 가져오는데 사용되는 키 0 이상으로 지정</param>
        public virtual void Set(ObjectManager.ObjectParams objectParams)
        {// DI 마렵군용  후후
            this.dataKey = objectParams.dataKey;
        }
    }
}