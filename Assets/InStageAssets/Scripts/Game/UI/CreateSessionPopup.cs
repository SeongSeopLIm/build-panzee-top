
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using WAK.Managers;
using Unity.Services.Multiplayer;
using Unity.Netcode;
using static System.Collections.Specialized.BitVector32;
using Cysharp.Threading.Tasks;
using Mono.Cecil.Cil;

namespace WAK.UI
{
    public class CreateSessionPopupData : PopupData
    { 

        public override void Initialize(int handleInstanceID, string viewID)
        {
            base.Initialize(handleInstanceID, viewID); 
        }

        protected override void OnShow()
        {
            base.OnShow(); 
        }

    }

    [UIView(id: nameof(CreateSessionPopup), path: "Prefabs/UI/CreateSessionPopup", dataType: typeof(CreateSessionPopupData))]
    public class CreateSessionPopup : Popup
    {
        [SerializeField] private GameObject sessionCreateRoot;
        [SerializeField] private GameObject sessionCreatedRoot;
        [SerializeField] private TMPro.TMP_InputField sessionNameField;
        [SerializeField] private Button createOrJoinSessionBtn;
        [SerializeField] private TMPro.TMP_Text sessionCode;
        [SerializeField] private Button sessionCopyBtn;
        



        CreateSessionPopupData PopupData => viewData as CreateSessionPopupData;


        protected override void AddListeners()
        {
            base.AddListeners();
            createOrJoinSessionBtn.onClick.AddListener(() => { OnClickCreateOrJoin().Forget(); });
            sessionCopyBtn.onClick.AddListener(() =>
            {
                GUIUtility.systemCopyBuffer = sessionCode.text;
            });
        }


        protected override void OnSetData(ViewData viewData)
        {
            base.OnSetData(viewData);

            MultiplayManager.Instance.SessionState
                .DistinctUntilChanged()
                .Select(state => state == SessionState.Connected)
                .Subscribe(isConnected =>
                {
                    sessionCreateRoot.SetActive(!isConnected);
                    sessionCreatedRoot.SetActive(isConnected);
                })
                .AddTo(this);
            MultiplayManager.Instance.SessionState
                .DistinctUntilChanged()
                .Where(state => state != SessionState.Connected)
                .Subscribe(_ =>
                {
                    sessionCode.text = "";
                })
                .AddTo(this);
            MultiplayManager.Instance.IsConnecting
                .DistinctUntilChanged()
                .Subscribe(isConnecting => createOrJoinSessionBtn.interactable = !isConnecting)
                .AddTo(this);

            sessionNameField.text = Guid.NewGuid().ToString();
        } 

        protected override void OnClear()
        {
            base.OnClear();
        }
         
        public void OnSessionJoined(ISession session)
        {
        }

        public void OnSessionJoinFailed(SessionException exception)
        {

        }
        public void OnSessionJoining()
        {

        }
        private async UniTaskVoid OnClickCreateOrJoin()
        {
            var session = await MultiplayManager.Instance.JoinOrJoinSession(Guid.NewGuid().ToString());

            sessionCode.text = session.Id;
        }

    }
}
