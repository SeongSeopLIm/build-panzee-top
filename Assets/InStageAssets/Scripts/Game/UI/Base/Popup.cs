using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAK.UI
{

    public class PopupData : WindowData
    {

    }

    [UIView(id: nameof(Popup), path: "Prefabs/UI/Popup", dataType: typeof(PopupData), isSingleInstance: true)]
    public class Popup : View
    { 
        protected override void OnSetData(ViewData viewData)
        {
            base.OnSetData(viewData);
        } 
    }
}
