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

        /// <param name="dataKey">������ Ǯ���� �������µ� ���Ǵ� Ű 0 �̻����� ����</param>
        public virtual void Set(ObjectManager.ObjectParams objectParams)
        { 
            this.ObjectParams = objectParams;
        }
    }
}