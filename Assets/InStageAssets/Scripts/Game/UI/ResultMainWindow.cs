
using UnityEngine;
using UnityEngine.UI;

namespace WAK.UI
{
    public class ResultMainWindowData : WindowData
    {

    }

    [UIView(id: nameof(ResultMainWindow), path: "Prefabs/UI/GameMainWindow", dataType: typeof(ResultMainWindowData))]
    public class ResultMainWindow : Window
    {
        [SerializeField] private Button restartBtn;
        [SerializeField] private Button toLobbyBtn;

        protected override void OnSetData(ViewData viewData)
        {
            base.OnSetData(viewData);
        }
    }
}
