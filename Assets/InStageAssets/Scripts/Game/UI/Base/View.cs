using DG.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using UnityEngine;
using WAK.Managers;

namespace WAK.UI
{
    public class UIViewAttribute: Attribute
    {
        public string ID { get; private set; }
        public string Path { get; private set; }
        public Type DataType { get; private set; }

        // 필요없을 것 같긴 한데, 일단 추가.
        public bool IsSingleInstance { get; private set; }
        public UIViewAttribute(string id, string path, Type dataType, bool isSingleInstance = true)
        {
            this.ID = id;
            this.Path = path;
            this.DataType = dataType;
            this.IsSingleInstance = isSingleInstance;   
        }
    }

    /// <summary>
    /// UIManager 핸들링 용도. 일반 클래스 사용 X 
    /// </summary>
    internal interface IViewHandler
    {
        void Initialize(ViewData viewData);
    } 

    internal interface IVisibleUpdater
    { 
        void Show();
        void Hide();
        void ShowWithAnimation(Tween showAnimation);
        void HideWithAnimation(Tween hideAnimation);
    }

    /// <summary>
    /// Data Model
    /// </summary>
    public class ViewData: IVisibleUpdater
    {
        ReactiveProperty<ViewState> state = new ReactiveProperty<ViewState>(ViewState.Hidden);
        IReadOnlyReactiveProperty<ViewState> State => state;



        #region UI view animation

        private void OnFinishTranslationToShow()
        {
            state.Value = ViewState.Hidden;
        }

        private void OnFinishTranslationToHide()
        {
            state.Value = ViewState.Show;
        }
        private void OnStartTranslationToShow()
        {
            state.Value = ViewState.StartTranslationToShow;
        }

        private void OnStartTranslationToHide()
        {
            state.Value = ViewState.StartTranslationToHide;
        }

        void IVisibleUpdater.ShowWithAnimation(Tween showAnimation)
        { 
            showAnimation.OnStart(OnStartTranslationToShow);
            showAnimation.OnComplete(OnFinishTranslationToShow);
            showAnimation.Play();
        }

        void IVisibleUpdater.HideWithAnimation(Tween hideAnimation)
        { 
            hideAnimation.OnStart(OnStartTranslationToHide);
            hideAnimation.OnComplete(OnFinishTranslationToHide);
            hideAnimation.Play();
        }

        void IVisibleUpdater.Show()
        {
            state.Value = ViewState.Show;
        }

        void IVisibleUpdater.Hide()
        {
            state.Value = ViewState.Hidden;
        }
        #endregion
    }

    /// <summary>
    /// Note : https://github.com/neuecc/UniRx - Model-View-(Reactive)Presenter Pattern
    /// </summary>
    [UIView(id: nameof(View), path: "Prefabs/UI/Window.prefab", dataType: typeof(ViewData),isSingleInstance: false)]
    public class View : MonoBehaviour, IViewHandler
    { 
        protected ViewData viewData { get; private set; }

        void IViewHandler.Initialize(ViewData viewData)
        {
            OnSetData(viewData);
            OnInitilized();
        }

        /// <summary>
        /// SetData 가 먼저 호출됨
        /// </summary>
        protected virtual void OnInitilized()
        {

        }

        protected virtual void OnSetData(ViewData viewData) 
        {
            this.viewData = viewData;
        }

    }
}
