
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using WAK.Managers;

namespace WAK.UI
{
    public class SettingPopupData : PopupData
    {

    }

    [UIView(id: nameof(SettingPopup), path: "Prefabs/UI/SettingPopup", dataType: typeof(SettingPopupData))]
    public class SettingPopup : Window
    {
        [SerializeField] private Slider BGMSlider;
        [SerializeField] private Slider SFXSlider;

        protected override void AddListeners()
        {
            base.AddListeners();
        }


        protected override void OnSetData(ViewData viewData)
        {
            base.OnSetData(viewData);
            
            BGMSlider
                .OnValueChangedAsObservable()
                .Subscribe()
                .AddTo(disposable);
            SFXSlider
                .OnValueChangedAsObservable()
                .Subscribe()
                .AddTo(disposable);
        }

        protected override void OnClear()
        {
            base.OnClear();
        }
    }
}
