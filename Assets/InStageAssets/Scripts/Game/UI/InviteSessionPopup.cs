
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
    public class InviteSessionPopupData : PopupData
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

    [UIView(id: nameof(InviteSessionPopup), path: "Prefabs/UI/InviteSessionPopup", dataType: typeof(InviteSessionPopupData))]
    public class InviteSessionPopup : Popup
    { 
        enum StatusType
        {
            None,
            CodeInvalid,
            RoomEmpty,
            Faield,
            Success
        }

        [SerializeField] private TMPro.TMP_InputField codeField;
        [SerializeField] private Button joinBtn;
        [SerializeField] private TMPro.TMP_Text statusText; 


        static string[] StatusTexts = new string[]{
            string.Empty,
            "코드를 입력해 주세요.",
            "없는 방입니다.",
            "참가에 실패했습니다.",
            string.Empty,
        };

        InviteSessionPopupData PopupData => viewData as InviteSessionPopupData;


        protected override void AddListeners()
        {
            base.AddListeners(); 
            joinBtn.onClick.AddListener(() => { OnClickJoin().Forget(); });
        }

        protected override void OnSetData(ViewData viewData)
        {
            base.OnSetData(viewData);
             
            MultiplayManager.Instance.IsConnecting
                .DistinctUntilChanged()
                .Subscribe(isConnecting => joinBtn.interactable = !isConnecting)
                .AddTo(this);

            statusText.text = StatusTexts[(int)StatusType.None];
            codeField.text = string.Empty;
        }

        protected override void OnClear()
        {
            base.OnClear();
        }

        private async UniTaskVoid OnClickJoin()
        {
            if(codeField.text.Equals(string.Empty))
            {
                statusText.text = "";
                return;
            }
            try
            {
                await MultiplayManager.Instance.JoinSession(codeField.text);
                statusText.text = StatusTexts[(int)StatusType.Success];
            }
            catch (Exception ex) when (ex is SessionException)
            { 
                statusText.text = StatusTexts[(int)StatusType.Faield]; 
            }
            
        }
         
    }
}
