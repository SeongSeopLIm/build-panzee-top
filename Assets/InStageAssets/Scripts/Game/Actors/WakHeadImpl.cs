using UniRx;

namespace WAK.Game
{
    public class WakHeadImpl : ActorImpl
    {
        public ReactiveProperty<bool> holdingAtCursor = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> isMoving = new ReactiveProperty<bool>(true);

        /// <summary>
        /// GameObjectComponent에서 직접 설정 필요
        /// </summary>
        public float TopPositionY = 0;


        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnSpawn(int objectID)
        {
            base.OnSpawn(objectID);
            TopPositionY = 0;
        }

        public override void OnDespawn()
        {
            TopPositionY = 0;
            base.OnDespawn();
        } 
    }
}