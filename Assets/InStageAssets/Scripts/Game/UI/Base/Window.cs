using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WAK.UI
{

    public class WindowData : ViewData
    {

    }

    [UIView(id: nameof(Window), path: "Prefabs/UI/Window", dataType: typeof(WindowData), isSingleInstance: false)]
    public class Window : View
    { 
        protected override void OnSetData(ViewData viewData)
        {
            base.OnSetData(viewData);
        }

        
    }
}
