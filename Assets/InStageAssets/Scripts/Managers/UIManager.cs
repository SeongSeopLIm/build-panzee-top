using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCommunity.UnitySingleton;
using Cysharp.Threading.Tasks;
using WAK.UI;
using System.Reflection;
using System;
using UniRx;

namespace WAK.Managers
{  
    public enum ViewState
    {
        Hidden,
        Show,
        StartTranslationToShow,
        StartTranslationToHide,
    }

    public class UIManager : PersistentMonoSingleton<UIManager>
    { // 고도화 없이 간단하게 페이지단위로 작성
        private GameObject instantiateRoot; 
        private Dictionary<string, ViewData> viewByID = new();
        private Dictionary<int, ViewData> viewByInstanceID = new();


        protected override void OnMonoSingletonCreated()
        {
            base.OnMonoSingletonCreated();
            instantiateRoot = new GameObject("instantiateRoot");
            instantiateRoot.transform.parent = this.transform;
            instantiateRoot.SetActive(false);
        }

        protected override void OnInitialized()
        {
            base.OnInitialized(); 
            GenerateMainWindowsByStage();
            Subscribe();
        }

        // UIManager에 있을 부분이 아니긴한데, 일단 작성.
        #region 인게임 페이지 컨트롤 영역
        private Dictionary<StageManager.StageType, WindowData> mainWindowByStage = new();
        StageManager.StageType curActiveStageWindow = StageManager.StageType.Spalsh;

        private void Subscribe()
        {
            StageManager.Instance.CurrentStageType
                .DistinctUntilChanged()
                .Subscribe(SwitchMainWindow)
                .AddTo(this);
        }

        public void SwitchMainWindow(StageManager.StageType nextStage)
        {
            if (mainWindowByStage.TryGetValue(curActiveStageWindow, out var prevWindow))
            {
                (prevWindow as IVisibleUpdater).Hide();
            }
            curActiveStageWindow = nextStage;
            if (mainWindowByStage.TryGetValue(curActiveStageWindow, out var nextWindow))
            {
                (nextWindow as IVisibleUpdater).Show();
            }
        }  

        private void GenerateMainWindowsByStage()
        {// TODO : SO로 하는 게 나았을 거 같긴 한데, 호다닥 하는 게 우선이니 일단 이렇게 진행.
            for (int i = 0; i < (int)StageManager.StageType.Max; i++)
            {
                var mainWindow = StageMainWindowFactory((StageManager.StageType)i) as WindowData;
                mainWindowByStage.TryAdd((StageManager.StageType)i, mainWindow);
            }
        }

        private ViewData StageMainWindowFactory(StageManager.StageType stageType)
        {
            return stageType switch
            {
                StageManager.StageType.Spalsh => GetOrCreateView<Window>(),
                StageManager.StageType.Lobby => GetOrCreateView<Window>(),
                StageManager.StageType.Play => GetOrCreateView<Window>(),
                StageManager.StageType.Result => GetOrCreateView<Window>(),
                StageManager.StageType.Max => throw new System.NotImplementedException(),
                _ => throw new System.NotImplementedException(),
            };
        }

        #endregion

        public void Show<T>() where T : View
        {
            var view = GetOrCreateView<T>();
        }

        public void Hide(string viewID)
        {

        }
         

        /// <param name="viewID">attribute에 지정된 viewID</param>
        private ViewData GetOrCreateView<T>() where T : View
        {
            var viewType = typeof(T);
            if(!viewByID.TryGetValue(nameof(viewType), out ViewData viewData))
            { 
                // OPTIMIZE : Attrubute도 캐생은 당장은 여기 스폰하는 데에서만 쓰이니 스킵. 추후 다중 인스턴스나, 여러곳에서 쓰이면 추가.
                var uiViewAttribute = viewType.GetCustomAttribute<UIViewAttribute>();
                if(uiViewAttribute == null)
                {
                    Debug.LogError($"UIViewAttribute 누락 : {nameof(viewType)}");
                    return null;
                }
                
                var prefab = Resources.Load<GameObject>(uiViewAttribute.Path);
                if(prefab == null)
                {
                    Debug.LogError($"Faield to find prefab : {uiViewAttribute.ID} | {uiViewAttribute.Path}");
                    return null;
                }
                var viewGO = Instantiate(prefab, instantiateRoot.transform);
                if(!viewGO.TryGetComponent<View>(out View view))
                {
                    Debug.LogError($"View Component 누락 : {nameof(viewType)}");
                    return null;
                }

                var viewhandler = view as IViewHandler;
                viewData = Activator.CreateInstance(uiViewAttribute.DataType) as ViewData;
                viewhandler.Initialize(viewData);
                   
                viewByInstanceID.TryAdd(viewGO.GetInstanceID(), viewData);
                viewByID.TryAdd(nameof(viewType), viewData);
            }

            return viewData;
        }

    } 
}

