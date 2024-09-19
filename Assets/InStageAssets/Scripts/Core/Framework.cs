using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCommunity;
using UnityCommunity.UnitySingleton;

namespace WAK.Core
{

    public class Framework : PersistentMonoSingleton<Framework>
    { 
        protected override void OnInitialized()
        {
            base.OnInitialized();
            PreloadSingletons();
        }

        private void PreloadSingletons()
        { 
            #region Singleton
            WAK.Managers.StageManager.CreateInstance();
            #endregion

            #region PersistentMonoSingleton
            Managers.SceneManager.CreateInstance();
            Managers.InputManger.CreateInstance();
            #endregion

            #region PersistentMonoSingleton

            #endregion
        }
    }

}
