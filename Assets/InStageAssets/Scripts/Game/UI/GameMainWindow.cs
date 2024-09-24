
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using WAK.Managers;

namespace WAK.UI
{


    [UIView(id: nameof(GameMainWindow), path: "Prefabs/UI/GameMainWindow", dataType: typeof(GameMainWindowData))]
    public class GameMainWindow : Window
    {
        private class GameMainWindowData : WindowData, IGameDataListener
        {
            [SerializeField] public ReactiveProperty<float> GameScore = new ReactiveProperty<float>(0);

            protected override void OnShow()
            {
                base.OnShow();
                GameManager.Instance.AddDataListener(this);
                GameScore.Value = GameManager.Instance.Score;
            }

            protected override void OnHide()
            {
                GameScore.Value = 0;
                GameManager.Instance.RemoveDataListener(this);
                base.OnHide();
            }

            void IGameDataListener.OnUpdate()
            { 
                GameScore.Value = GameManager.Instance.Score;
            }
        }


        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Button backBtn;
        GameMainWindowData GameWindowData => viewData as GameMainWindowData;


        protected override void AddListeners()
        {
            base.AddListeners();
            backBtn.onClick.AddListener(OnClickBack); 
        }

        protected override void OnSetData(ViewData viewData)
        {
            base.OnSetData(viewData);
        }

        protected override void OnInitilized()
        {
            base.OnInitilized();
            
            GameWindowData.GameScore
                .Subscribe(score => scoreText.text = $"{score:0.0} M") 
                .AddTo(disposable);
        }

        private void OnClickBack()
        {
            StageManager.Instance.SwitchStage(StageManager.StageType.Lobby);
        }
    }
}
