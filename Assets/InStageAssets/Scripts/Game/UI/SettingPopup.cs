
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
        public ReactiveProperty<float> BGMVolume = new ReactiveProperty<float>();
        public ReactiveProperty<float> SFXVolume = new ReactiveProperty<float>();

        public override void Initialize(int handleInstanceID, string viewID)
        {
            base.Initialize(handleInstanceID, viewID);
            BGMVolume.Value = AudioManager.Instance.BGMVolume;
            SFXVolume.Value = AudioManager.Instance.SFXVolume;
        }

        protected override void OnShow()
        {
            base.OnShow();
            BGMVolume.Value = AudioManager.Instance.BGMVolume;
            SFXVolume.Value = AudioManager.Instance.SFXVolume;
        }

    }

    [UIView(id: nameof(SettingPopup), path: "Prefabs/UI/SettingPopup", dataType: typeof(SettingPopupData))]
    public class SettingPopup : Popup
    {
        [SerializeField] private Slider BGMSlider;
        [SerializeField] private Slider SFXSlider;
        [SerializeField] private TMP_Text BGMVolumeValue;
        [SerializeField] private TMP_Text SFXVolumeValue;

        SettingPopupData settingPopupData => viewData as SettingPopupData;

        protected override void OnSetData(ViewData viewData)
        {
            base.OnSetData(viewData);

            settingPopupData.BGMVolume
                .DistinctUntilChanged()
                .Subscribe(value =>
                {
                    BGMSlider.value = value;
                    UpdateBGMVolumeText(value);
                })
                .AddTo(disposable);

            settingPopupData.SFXVolume
                .DistinctUntilChanged()
                .Subscribe(value =>
                {
                    SFXSlider.value = value;
                    UpdateSFXVolumeText(value);
                })
                .AddTo(disposable);


            BGMSlider
                .OnValueChangedAsObservable()
                .DistinctUntilChanged()
                .Subscribe(value =>
                {
                    AudioManager.Instance.SetBGMVolume(value);
                    settingPopupData.BGMVolume.Value = value;
                })
                .AddTo(disposable);
            SFXSlider
                .OnValueChangedAsObservable()
                .DistinctUntilChanged()
                .Subscribe(value =>
                {
                    AudioManager.Instance.SetSFXVolume(value);
                    settingPopupData.SFXVolume.Value = value;
                })
                .AddTo(disposable);
        }

        private void UpdateBGMVolumeText(float value)
        {
            int percentage = Mathf.RoundToInt(value * 100);
            BGMVolumeValue.text = $"{percentage}%";
        }


        private void UpdateSFXVolumeText(float value)
        {
            int percentage = Mathf.RoundToInt(value * 100);
            SFXVolumeValue.text = $"{percentage}%";
        }

        protected override void OnClear()
        {
            base.OnClear();
        }
    }
}
