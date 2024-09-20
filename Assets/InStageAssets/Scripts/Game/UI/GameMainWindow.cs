
using UnityEngine;
using UnityEngine.UI;

namespace WAK.UI
{
    public class GameMainWindowData : WindowData
    {

    }

    [UIView(id: nameof(GameMainWindow), path: "Prefabs/UI/GameMainWindow", dataType: typeof(GameMainWindowData))]
    public class GameMainWindow : Window
    {
        [SerializeField] private Button backBtn;

        protected override void OnSetData(ViewData viewData)
        {
            base.OnSetData(viewData);
        }
    }
}
