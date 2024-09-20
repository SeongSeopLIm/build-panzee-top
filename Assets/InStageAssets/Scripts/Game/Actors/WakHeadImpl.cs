using UniRx;

namespace WAK.Game
{
    public class WakHeadImpl : ActorImpl
    {
        public ReactiveProperty<bool> holdingAtCursor = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> isMoving = new ReactiveProperty<bool>(true);


        public override void OnCreated()
        {
            base.OnCreated();
        }

        public override void OnSpawn(int objectID)
        {
            base.OnSpawn(objectID);

        }

        public override void OnDespawn()
        {
            
            base.OnDespawn();
        } 
    }
}