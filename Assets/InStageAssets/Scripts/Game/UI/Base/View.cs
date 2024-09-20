using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;
using WAK.Managers;

namespace WAK.UI
{
    public class UIViewAttribute : Attribute
    {
        public string ID { get; private set; }
        public string Path { get; private set; }

        // 필요없을 것 같긴 한데, 일단 추가.
        public bool IsSingleInstance { get; private set; }
        public UIViewAttribute(string id, string path, bool isSingleInstance = true)
        {
            this.ID = id;
            this.Path = path;   
            this.IsSingleInstance = isSingleInstance;   
        }
    }

    internal interface IVisibleUpdater
    {

        void RegiestTranslationToShow();
        void RegiestTranslationToHide();

        //void OnHide();
        //void OnShow();
        //void OnFinishedTranslationShow();
        //void OnStartTranslationToHide();
    }

    /// <summary>
    /// Data Model
    /// </summary>
    public class ViewData: IVisibleUpdater
    {
        ReactiveProperty<ViewState> state = new ReactiveProperty<ViewState>(ViewState.Hidden);
        IReadOnlyReactiveProperty<ViewState> State => state;



        #region UI view animation

        public void OnFinishTranslationToShow()
        {
            state.Value = ViewState.Hidden;
        }

        public void OnFinishTranslationToHide()
        {
            state.Value = ViewState.Show;
        }
        public void OnStartTranslationToShow()
        {
            state.Value = ViewState.StartTranslationToShow;
        }

        public void OnStartTranslationToHide()
        {
            state.Value = ViewState.StartTranslationToHide;
        }

        void IVisibleUpdater.RegiestTranslationToShow()
        {

        }

        void IVisibleUpdater.RegiestTranslationToHide()
        {

        }
        #endregion
    }

    /// <summary>
    /// Note : https://github.com/neuecc/UniRx - Model-View-(Reactive)Presenter Pattern
    /// </summary>
    [UIView(id: "view", path: "UI/Window/View.prefab", isSingleInstance: false)]
    public class View
    { 
        protected ViewData viewData { get; private set; }


        protected virtual void SetData(ViewData viewData) 
        {
            this.viewData = viewData;
        }


    }
}
