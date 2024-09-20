using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem;
using UnityEngine;
using WAK.Managers;
using UniRx;
using System;

namespace WAK.Game
{
    /// <summary>
    /// ��ġ���� ���� ������� ������Ʈ 
    /// </summary>
    [ActorImpl(poolType: "WakHead", prefabRelativePath: "/WakHead.prefab")]
    public class WakHead : Actor
    {
        // rigidbody2D �̸��� �ȵǴµ� ��¡? ���� �׷� �ǰ�
        [SerializeField] private Rigidbody2D _rigidbody2D;

        protected WakHeadImpl wakHeadimpl => impl as WakHeadImpl;
        CompositeDisposable disposables = new CompositeDisposable();
        public override void OnSpawn()
        {
            base.OnSpawn();
            wakHeadimpl.holdingAtCursor
                .DistinctUntilChanged()
                .Subscribe(OnHoldToCursor)
                .AddTo(disposables);
        }

        public override void OnDespawn()
        {
            disposables?.Clear();
            base.OnDespawn();
        }


        private void OnHoldToCursor(bool isHold)
        {
            _rigidbody2D.simulated = !isHold; 
        }


        private void Update()
        {
            if (wakHeadimpl.holdingAtCursor.Value)
            {
                transform.position = GameManager.Instance.GetCursorHoldPosition(); 
            }
            CheckIsMoving();
        }

        private void CheckIsMoving()
        {
            // TODO : üũ ��� ���� �ʿ�. 
            if (_rigidbody2D.velocity.sqrMagnitude > 0.1 ||
                Mathf.Abs(_rigidbody2D.angularVelocity) > 0.1)
            {
                wakHeadimpl.isMoving.Value = true;
            }
            else
            {
                wakHeadimpl.isMoving.Value = false;
            }
        }



    }
}