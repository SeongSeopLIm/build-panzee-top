
using UnityEngine;
using UnityEngine.UI;

namespace WAK.UI
{
    public class LobbyMainWindowData : WindowData
    {

    }

    public class LobbyMainWindow : Window
    {
        [SerializeField] private Button startBtn;

        protected override void SetData(ViewData viewData)
        {
            base.SetData(viewData);
        }
    }
}
