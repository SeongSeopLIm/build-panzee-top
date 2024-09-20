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

    [UIView(id: "Window", path: "UI/Window/Window.prefab", isSingleInstance: false)]
    public class Window : View
    { 
        protected override void SetData(ViewData viewData)
        {
            base.SetData(viewData);
        }

        
    }
}
