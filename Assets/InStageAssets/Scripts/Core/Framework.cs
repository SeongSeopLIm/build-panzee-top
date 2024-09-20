using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCommunity;
using UnityCommunity.UnitySingleton;
using WAK.Managers;
using UniRx;
using WAK.Game;

namespace WAK.Core
{

    public class Framework : PersistentMonoSingleton<Framework>
    {
        [SerializeField] private GlobalSettings globalSettings;
        public GlobalSettings GlobalSettings => globalSettings;

        private ReactiveProperty<bool> isApplicationReady = new ReactiveProperty<bool>(false);
        public IReadOnlyReactiveProperty<bool> IsApplicationReady => isApplicationReady;

        protected override void OnInitializing()
        {
            base.OnInitializing();
        }

        protected override void OnInitialized()
        {
            base.OnInitialized();
            isApplicationReady.Value = false;
            PreloadSingletons(); 
            StartApplication();
        }

        /// <summary>
        /// ΩÃ±€≈Ê ∏≈¥œ¿˙ πÃ∏Æ ∑ŒµÂ. « ø‰«— ΩÃ±€≈Ê∏∏ √ﬂ∞°.
        /// </summary>
        private void PreloadSingletons()
        { 
            #region Singleton

            #endregion

            #region PersistentMonoSingleton
            Managers.UnityGameSceneManager.CreateInstance(); 
            #endregion 
        } 

        private void StartApplication()
        {
            StageManager.Instance.SwitchStage(StageManager.StageType.Spalsh);
            
            isApplicationReady.Value =true;
        }
    }

}
