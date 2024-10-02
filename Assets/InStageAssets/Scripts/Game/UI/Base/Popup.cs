using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using WAK.Managers;

namespace WAK.UI
{

    public class PopupData : WindowData
    {

    }

    [UIView(id: nameof(Popup), path: "Prefabs/UI/Popup", dataType: typeof(PopupData), isSingleInstance: true)]
    public class Popup : View
    {
        [SerializeField] private Button closeBtn;
        [SerializeField] private Button BG;

        // TODO : UI 매니저에 팝업 관리 영역 추가... 근데 좀 귀찮다... 필요한 앱도 아니니 우선 이대로 ㄱㄱ


        protected override void AddListeners()
        {
            base.AddListeners();
            if (closeBtn)
            {
                closeBtn.onClick.AddListener(OnClickClose);
            }
            if (BG)
            {
                BG.onClick.AddListener(OnClickBG);
            }
        }

        private void OnClickBG()
        {
            UIManager.Instance.Hide(viewData.ViewID);
        }

        private void OnClickClose()
        {
            UIManager.Instance.Hide(viewData.ViewID);
        }

        protected override void OnSetData(ViewData viewData)
        {
            base.OnSetData(viewData);
        } 
    }
}
