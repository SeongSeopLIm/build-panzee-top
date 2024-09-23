
using System;
using UnityEngine;
using UnityEngine.UI;
using WAK.Managers;

namespace WAK.UI
{
    public class ResultMainWindowData : WindowData
    {

    }

    [UIView(id: nameof(ResultMainWindow), path: "Prefabs/UI/ResultMainWindow", dataType: typeof(ResultMainWindowData))]
    public class ResultMainWindow : Window
    {
        [SerializeField] private Button restartBtn;
        [SerializeField] private Button toLobbyBtn;

        protected override void AddListeners()
        {
            base.AddListeners();
            restartBtn.onClick.AddListener(OnClickRestart);
            toLobbyBtn.onClick.AddListener(OnClickToLobbyBtn);
        }

        protected override void OnSetData(ViewData viewData)
        {
            base.OnSetData(viewData);
        }
        private void OnClickToLobbyBtn()
        {
            StageManager.Instance.SwitchStage(StageManager.StageType.Lobby);
        }

        private void OnClickRestart()
        {
            StageManager.Instance.SwitchStage(StageManager.StageType.Play);
        }

    }
}
