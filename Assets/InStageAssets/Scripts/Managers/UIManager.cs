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
        private GameObject disabledRoot;
        [SerializeField] private GameObject activeRoot; // 우선 UI오더 고려 없이 제작. 

        private Dictionary<string, ViewData> viewDataByID = new();
        private Dictionary<int, View> viewByInstanceID = new();
         
        protected override void OnInitialized()
        {
            base.OnInitialized();
            disabledRoot = new GameObject("disabledRoot");
            var rt = disabledRoot.AddComponent<RectTransform>();
            rt.SetParent(gameObject.transform);
            rt.localPosition = Vector3.zero;
            rt.localScale = Vector3.one;
            rt.anchorMin = Vector2.zero;
            rt.anchorMax = Vector2.one;
            disabledRoot.SetActive(false);

            //activeRoot = new GameObject("activeRoot");
            //rt = activeRoot.AddComponent<RectTransform>();
            //rt.SetParent(gameObject.transform);
            //rt.localPosition = Vector3.zero;
            //rt.localScale = Vector3.one;
            //rt.anchorMin = Vector2.zero;
            //rt.anchorMax = Vector2.one;
            activeRoot.SetActive(true);


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
                .Subscribe(SwitchMainWindow);
        }

        public void SwitchMainWindow(StageManager.StageType nextStage)
        {
            if (mainWindowByStage.TryGetValue(curActiveStageWindow, out var prevWindow))
            {
                Hide(prevWindow.ViewID); 
            }
            curActiveStageWindow = nextStage;
            if (mainWindowByStage.TryGetValue(curActiveStageWindow, out var nextWindow))
            {
                Show(nextWindow.ViewID); 
            }
        }  

        private void GenerateMainWindowsByStage()
        {// TODO : SO로 하는 게 나았을 거 같긴 한데, 호다닥 하는 게 우선이니 일단 이렇게 진행.
            for (int i = 0; i < (int)StageManager.StageType.Max; i++)
            {
                var mainWindow = MainViewFactory((StageManager.StageType)i) as WindowData;
                mainWindowByStage.TryAdd((StageManager.StageType)i, mainWindow);
            }
        }

        private ViewData MainViewFactory(StageManager.StageType stageType)
        {
            return stageType switch
            {
                StageManager.StageType.Spalsh => GetOrCreateView<Window>(),
                StageManager.StageType.Lobby => GetOrCreateView<LobbyMainWindow>(),
                StageManager.StageType.Play => GetOrCreateView<GameMainWindow>(),
                StageManager.StageType.Result => GetOrCreateView<ResultMainWindow>(),
                StageManager.StageType.Max => throw new System.NotImplementedException(),
                _ => throw new System.NotImplementedException(),
            };
        }

        #endregion
         

        public void Show<T>() where T : View
        {
            var viewData = GetOrCreateView<T>();

            var viewTransfrom = viewByInstanceID[viewData.HandleInstanceID].gameObject.transform;
            viewTransfrom.SetParent(activeRoot.transform);
            viewTransfrom.localPosition = Vector3.zero;
            viewTransfrom.localScale = Vector3.one;
            (viewData as IVisibleUpdater).Show();
            
        }

        public void Show(string viewID)
        { 
            if (viewDataByID.TryGetValue(viewID, out ViewData viewData))
            {
                viewByInstanceID[viewData.HandleInstanceID].gameObject.TryGetComponent<RectTransform>(out var viewTransfrom);
                viewTransfrom.SetParent(activeRoot.transform);
                viewTransfrom.localPosition = Vector3.zero;
                viewTransfrom.localScale = Vector3.one;
                viewTransfrom.sizeDelta = Vector3.zero;

                (viewData as IVisibleUpdater).Show();
            }
            else
            {
                Debug.LogError($"Faield Find viewID : {viewID}");
            }
        }

        public void Hide(string viewID)
        { 
            if (viewDataByID.TryGetValue(viewID, out ViewData viewData))
            {
                (viewData as IVisibleUpdater).Hide();

                var viewTransfrom = viewByInstanceID[viewData.HandleInstanceID].gameObject.transform;
                viewTransfrom.SetParent(disabledRoot.transform);
                viewTransfrom.localPosition = Vector3.zero;
                viewTransfrom.localScale = Vector3.one;
            }
            else
            {
                Debug.LogError($"Faield Find viewID : {viewID}");
            }
        }



        /// <param name="viewID">attribute에 지정된 viewID</param>
        private ViewData GetOrCreateView<T>() where T : View
        {
            var viewType = typeof(T);
            // TODO : 아직 다중 인스턴스 고려 X. 필요하면 그때 확장.
            if(!viewDataByID.TryGetValue(viewType.Name, out ViewData viewData))
            { 
                // OPTIMIZE : Attrubute도 캐생은 당장은 여기 스폰하는 데에서만 쓰이니 스킵. 추후 다중 인스턴스나, 여러곳에서 쓰이면 추가.
                var uiViewAttribute = viewType.GetCustomAttribute<UIViewAttribute>();
                if(uiViewAttribute == null)
                {
                    Debug.LogError($"UIViewAttribute 누락 : {viewType.Name}");
                    return null;
                }
                
                var prefab = Resources.Load<GameObject>(uiViewAttribute.Path);
                if(prefab == null)
                {
                    Debug.LogError($"Faield to find prefab : {uiViewAttribute.ID} | {uiViewAttribute.Path}");
                    return null;
                }
                var viewGO = Instantiate(prefab, disabledRoot.transform);
                if(!viewGO.TryGetComponent<View>(out View view))
                {
                    Debug.LogError($"View Component 누락 : {viewType.Name}");
                    return null;
                }

                var viewhandler = view as IViewHandler;
                viewData = Activator.CreateInstance(uiViewAttribute.DataType) as ViewData;

                viewData.Initialize(viewGO.GetInstanceID(), viewType.Name);
                viewhandler.Initialize(viewData);
                   
                viewByInstanceID.TryAdd(viewGO.GetInstanceID(), view);
                viewDataByID.TryAdd(viewType.Name, viewData);
            }

            return viewData;
        }

    } 
}

