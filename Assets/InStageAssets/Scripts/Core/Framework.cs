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
            int targetWidth = Screen.width;
            int minHeight = Mathf.RoundToInt(targetWidth * (16.0f / 9.0f));
            var height = Mathf.Max(minHeight, 1920); 
            int targetHeight = Mathf.Max(Screen.height, height);

            Screen.SetResolution(targetWidth, targetHeight, true);
            Application.targetFrameRate = 144;

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
            MultiplayManager.CreateInstance();
            #endregion 
        } 

        private void StartApplication()
        {
            StageManager.Instance.SwitchStage(StageManager.StageType.Spalsh);
            
            isApplicationReady.Value =true;
        }
    }

}
