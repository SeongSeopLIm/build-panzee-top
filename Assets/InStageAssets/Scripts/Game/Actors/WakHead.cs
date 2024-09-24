using static UnityEngine.GraphicsBuffer;
using UnityEngine.InputSystem;
using UnityEngine;
using WAK.Managers;
using UniRx;
using System;
using DG.Tweening;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

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

        Vector2 spriteSize;
        protected WakHeadImpl wakHeadimpl => impl as WakHeadImpl;
        CompositeDisposable disposables = new CompositeDisposable();

        public override void OnSpawn()
        {
            base.OnSpawn();
            wakHeadimpl.holdingAtCursor
                .DistinctUntilChanged()
                .Subscribe(OnHoldChangedToCursor)
                .AddTo(disposables);
            wakHeadimpl.roateMode
                .DistinctUntilChanged()
                .Subscribe(OnRotationModeChanged)
                .AddTo(disposables); 
            spriteSize = spriteRenderer.bounds.size;
        }

        private void OnRotationModeChanged(WakHeadImpl.RotationMode mode)
        {
            DOTween.Kill(GetInstanceID());

            if(mode == WakHeadImpl.RotationMode.Stop)
                return;
            var dir = mode == WakHeadImpl.RotationMode.Right ? 1 : -1;
            var speed = GameManager.Instance.GameSettings.SpinAnglePerSec;

            transform.DOLocalRotate(new Vector3(0, 0, 360f * dir), speed, RotateMode.LocalAxisAdd)
                .SetSpeedBased()
                .SetLoops(-1)
                .SetEase(Ease.Linear)
                .SetId(GetInstanceID());
        }

        public override void OnDespawn()
        {
            DOTween.Kill(GetInstanceID());
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

        private void OnHoldChangedToCursor(bool isHold)
        {
            _rigidbody2D.simulated = !isHold; 
            if(isHold is false)
            {
                wakHeadimpl.roateMode.Value = WakHeadImpl.RotationMode.Stop;
            }
        }

        private void Update()
        {
            if (wakHeadimpl.holdingAtCursor.Value)
            {
                
                transform.position = new Vector3(0, spriteSize.y / 2, 0) + GameManager.Instance.GetCursorHoldPosition(); 
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
                    wakHeadimpl.isMoving.Value = false;
                    OnStop();
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
            GameManager.Instance.CheckRespawn(); 
        }
    }
}