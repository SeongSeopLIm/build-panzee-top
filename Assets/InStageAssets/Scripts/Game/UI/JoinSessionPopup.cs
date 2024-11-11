
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using WAK.Managers;
using Unity.Services.Multiplayer;
using Unity.Netcode;

namespace WAK.UI
{
    public class JoinSessionPopupData : PopupData
    {
        public ReactiveProperty<bool> isSessionCreated = new ReactiveProperty<bool>(false);

        public override void Initialize(int handleInstanceID, string viewID)
        {
            base.Initialize(handleInstanceID, viewID); 
        }

        protected override void OnShow()
        {
            base.OnShow(); 
        }

    }

    [UIView(id: nameof(JoinSessionPopup), path: "Prefabs/UI/JoinSessionPopup", dataType: typeof(JoinSessionPopupData))]
    public class JoinSessionPopup : Popup
    {
        [SerializeField] private GameObject sessionCreateRoot;
        [SerializeField] private GameObject sessionCreatedRoot;


        JoinSessionPopupData PopupData => viewData as JoinSessionPopupData;
         

        protected override void OnSetData(ViewData viewData)
        {
            base.OnSetData(viewData);
            MultiplayerService.Instance.GetJoinedSessionIdsAsync().Wait();
            PopupData.isSessionCreated
                .DistinctUntilChanged()
                .Subscribe(isOn =>
                {
                    sessionCreateRoot.SetActive(!isOn);
                    sessionCreatedRoot.SetActive(isOn);
                }); 
            
        } 

        protected override void OnClear()
        {
            base.OnClear();
        }
         

    }
}
