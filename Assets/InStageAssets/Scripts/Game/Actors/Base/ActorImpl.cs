namespace WAK
{

    [ActorImpl(actorID: "Actor", prefabRelativePath: "/ActorBase.prefab")]
    public class ActorImpl
    {
        /// <summary>
        /// GameObject.InstanceID
        /// </summary>
        public int ObjectID { get; private set; }

        public ObjectManager.ObjectParams ObjectParams { get; private set; }

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
        { 
            this.ObjectParams = objectParams;
        }
    }
}