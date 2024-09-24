using UniRx;

namespace WAK.Game
{
    public class WakHeadImpl : ActorImpl
    {
        public enum RotationMode
        {
            Stop,
            Left,//�ݽð�
            Right,//�ð����
        }
        public ReactiveProperty<bool> holdingAtCursor = new ReactiveProperty<bool>(true);
        public ReactiveProperty<bool> isMoving = new ReactiveProperty<bool>(true);
        // ������Ʈ Ȧ���߿��� ����
        public ReactiveProperty<RotationMode> roateMode = new ReactiveProperty<RotationMode>(RotationMode.Stop);

        /// <summary>
        /// GameObjectComponent���� ���� ���� �ʿ�
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