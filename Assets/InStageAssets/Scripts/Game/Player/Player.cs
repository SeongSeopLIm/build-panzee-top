using DG.Tweening;
using System;
using UniRx;
using UnityEngine;
using WAK.Managers;

namespace WAK
{
    public class Player : MonoBehaviour, IGameDataListener
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private float yPosFromMaxHeight;
        public Camera PlayerCamera => playerCamera;
        private Vector3 originPos;


        private void Start()
        {
            originPos = transform.position;
            GameManager.Instance.AddDataListener(this);
            StageManager.Instance.CurrentStageType
                .Subscribe(OnStageChagned)
                .AddTo(this);
        }


        void IGameDataListener.OnUpdate()
        {
            var stageType = StageManager.Instance.CurrentStageType.Value; 
            if (stageType != StageManager.StageType.Play)
                return;

            UpdateCameraPosition();
        }

        private void OnStageChagned(StageManager.StageType type)
        {
            switch (type)
            {
                case StageManager.StageType.Spalsh:
                case StageManager.StageType.Lobby:
                case StageManager.StageType.Play:
                    ResetCameraPosition();
                    break;
                case StageManager.StageType.Result:
                    break; 
                default:
                    Debug.LogError("미구현");
                    break;
            }

        }

        private void ResetCameraPosition()
        {
            DOTween.Kill(GetInstanceID());
            transform.localPosition = originPos;
        }

        private void UpdateCameraPosition()
        {
            var newY = yPosFromMaxHeight + GameManager.Instance.CurrentTopHeight;
            //초기값 미만 스킵
            if (newY < originPos.y)
                return;

            DOTween.Kill(GetInstanceID());
            transform.DOMoveY(newY, Constants.DEFAULT_ANIMATION_DURATION_CAMEAR)
                .SetEase(Ease.InOutQuad)
                .SetId(GetInstanceID());
        }
    }
}
