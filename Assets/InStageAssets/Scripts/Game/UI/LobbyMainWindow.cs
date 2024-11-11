
using System;
using TMPro;
using Unity.Services.Multiplayer;
using UnityEngine;
using UnityEngine.UI;
using WAK.Managers;

namespace WAK.UI
{
    public class LobbyMainWindowData : WindowData
    {

    }

    [UIView(id: nameof(LobbyMainWindow), path: "Prefabs/UI/LobbyMainWindow", dataType: typeof(LobbyMainWindowData))]
    public class LobbyMainWindow : Window
    {
        [SerializeField] private Button startBtn;
        [SerializeField] private Button createSessonBtn;
        [SerializeField] private Button joinSesisonBtn;
        [SerializeField] private Button settingBtn; 

        protected override void AddListeners()
        {
            base.AddListeners();
            startBtn.onClick.AddListener(OnClickStart);
            createSessonBtn.onClick.AddListener(OnClickCreateSession);
            joinSesisonBtn.onClick.AddListener(OnClickJoinSession);
            settingBtn.onClick.AddListener(OnClickOpenSetting);
        }


        private void OnClickOpenSetting()
        {
            UIManager.Instance.Show<SettingPopup>();
        }

        protected override void OnSetData(ViewData viewData)
        {
            base.OnSetData(viewData);
        }

        private void OnClickStart()
        {
            StageManager.Instance.SwitchStage(StageManager.StageType.Play);
        }

        private void OnClickJoinSession()
        {
            UIManager.Instance.Show<InviteSessionPopup>();
        }

        private void OnClickCreateSession()
        {
            UIManager.Instance.Show<CreateSessionPopup>();
        }
    }
}
