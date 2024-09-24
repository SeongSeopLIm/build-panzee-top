
using System;
using TMPro;
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

        protected override void AddListeners()
        {
            base.AddListeners();
            startBtn.onClick.AddListener(OnClickStart);
        }


        protected override void OnSetData(ViewData viewData)
        {
            base.OnSetData(viewData);
        }

        private void OnClickStart()
        {
            StageManager.Instance.SwitchStage(StageManager.StageType.Play);
        }
    }
}
