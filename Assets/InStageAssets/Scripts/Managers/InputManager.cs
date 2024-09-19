using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCommunity.UnitySingleton;
using UnityEngine.InputSystem;

namespace WAK.Managers
{
    public class InputManger : PersistentMonoSingleton<InputManger>, MainControl.IPlayActions
    {
        [SerializeField] private PlayerInput playerInput;

        private MainControl mainControl;
        protected override void OnInitialized()
        {
            base.OnInitialized();


            // MainControl 인스턴스 생성
            mainControl = new MainControl(); 
            mainControl.play.SetCallbacks(this); 
            mainControl.Enable();
        }

        private void OnDestroy()
        {
            if (mainControl != null)
            { 
                mainControl.play.RemoveCallbacks(this); 
                mainControl.Disable(); 
                mainControl.Dispose();
            }
        }

        void MainControl.IPlayActions.OnTap(InputAction.CallbackContext context)
        {

        }
    }
}

