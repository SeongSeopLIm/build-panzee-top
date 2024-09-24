using UniRx;

namespace WAK.Game
{
    public class WakHeadImpl : ActorImpl
    {
        public enum RotationMode
        {
            Stop,
            Left,//반시계
            Right,//시계방향
        }
        public ReactiveProperty<bool> holdingAtCursor = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> isMoving = new ReactiveProperty<bool>(true);
        // 오브젝트 홀딩중에만 동작
        public ReactiveProperty<RotationMode> roateMode = new ReactiveProperty<RotationMode>(RotationMode.Stop);

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
            holdingAtCursor.Value = true;
            isMoving.Value = true;
            roateMode.Value = RotationMode.Stop;
        }

        public override void OnDespawn()
        {
            TopPositionY = 0;
            roateMode.Value = RotationMode.Stop;
            base.OnDespawn();
        } 
    }
}