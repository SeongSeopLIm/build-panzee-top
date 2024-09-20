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
    { // ��ȭ ���� �����ϰ� ������������ �ۼ�
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

        // UIManager�� ���� �κ��� �ƴϱ��ѵ�, �ϴ� �ۼ�.
        #region �ΰ��� ������ ��Ʈ�� ����
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
        {// TODO : SO�� �ϴ� �� ������ �� ���� �ѵ�, ȣ�ٴ� �ϴ� �� �켱�̴� �ϴ� �̷��� ����.
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
         

        /// <param name="viewID">attribute�� ������ viewID</param>
        private ViewData GetOrCreateView<T>() where T : View
        {
            var viewType = typeof(T);
            if(!viewByID.TryGetValue(nameof(viewType), out ViewData viewData))
            { 
                // OPTIMIZE : Attrubute�� ĳ���� ������ ���� �����ϴ� �������� ���̴� ��ŵ. ���� ���� �ν��Ͻ���, ���������� ���̸� �߰�.
                var uiViewAttribute = viewType.GetCustomAttribute<UIViewAttribute>();
                if(uiViewAttribute == null)
                {
                    Debug.LogError($"UIViewAttribute ���� : {nameof(viewType)}");
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
                    Debug.LogError($"View Component ���� : {nameof(viewType)}");
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

