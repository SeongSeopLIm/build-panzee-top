
using UnityEngine;
using UnityEngine.UI;

namespace WAK.UI
{
    public class LobbyMainWindowData : WindowData
    {

    }

    [UIView(id: nameof(LobbyMainWindow), path: "Prefabs/UI/LobbyMainWindow", dataType: typeof(LobbyMainWindowData))]
    public class LobbyMainWindow : Window
    {
        [SerializeField] private Button startBtn;

        protected override void OnSetData(ViewData viewData)
        {
            base.OnSetData(viewData);
        }
    }
}
