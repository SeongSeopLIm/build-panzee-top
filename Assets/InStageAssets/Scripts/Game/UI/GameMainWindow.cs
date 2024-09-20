
using System;
using UnityEngine;
using UnityEngine.UI;
using WAK.Managers;

namespace WAK.UI
{
    public class GameMainWindowData : WindowData
    {

    }

    [UIView(id: nameof(GameMainWindow), path: "Prefabs/UI/GameMainWindow", dataType: typeof(GameMainWindowData))]
    public class GameMainWindow : Window
    {
        [SerializeField] private Button backBtn;

        protected override void AddListeners()
        {
            base.AddListeners();
            backBtn.onClick.AddListener(OnClickBack);
        }

        protected override void OnSetData(ViewData viewData)
        {
            base.OnSetData(viewData);
        }

        private void OnClickBack()
        {
            StageManager.Instance.SwitchStage(StageManager.StageType.Lobby);
        }
    }
}
