using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem;
using UnityEngine;
using WAK.Managers;
using UniRx;
using System;

namespace WAK.Game
{
    /// <summary>
    /// 위치값을 갖는 월드상의 오브젝트 
    /// </summary>
    [ActorImpl(actorID: "WakHead", prefabRelativePath: "/WakHead.prefab"),
        RequireComponent(typeof(SpriteRenderer), typeof(PolygonCollider2D), typeof(Rigidbody2D))]
    public class WakHead : Actor
    {
        // rigidbody2D 이름이 안되는데 모징? 원래 그런 건가
        [SerializeField] private Rigidbody2D _rigidbody2D; 
        [SerializeField] private SpriteRenderer spriteRenderer; 
        [SerializeField] private Collider2D _collider2D;


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
         
        private void OnValidate()
        {
            if (!_rigidbody2D)
            {
                gameObject.TryGetComponent(out _rigidbody2D);
            }
            if (!spriteRenderer)
            {
                gameObject.TryGetComponent(out spriteRenderer);
            }
            if (!_collider2D)
            {
                gameObject.TryGetComponent(out _collider2D);
            }
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
            else
            {
                CheckIsMoving();
            }
            
        }

        private void CheckIsMoving()
        {
            var filter = new ContactFilter2D() { layerMask = gameObject.layer }; 
            // TODO : 체크 방식 수정 필요. 
            if (_rigidbody2D.velocity.sqrMagnitude > 0.1 ||
                Mathf.Abs(_rigidbody2D.angularVelocity) > 0.1 ||
                !_collider2D.IsTouching(filter))
            {
                if(!wakHeadimpl.isMoving.Value)
                    wakHeadimpl.isMoving.Value = true;
            }
            else
            {
                if (wakHeadimpl.isMoving.Value)
                {
                    OnStop();
                    wakHeadimpl.isMoving.Value = false;
                }
            }
        }

        private void OnStop()
        {
            wakHeadimpl.TopPositionY = spriteRenderer.bounds.max.y;
            if(GameManager.Instance.CurrentTopHeight < wakHeadimpl.TopPositionY)
            {
                GameManager.Instance.RegisterHighestObject(wakHeadimpl);
            }
            
        }
    }
}