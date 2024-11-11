using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCommunity.UnitySingleton;
using Cysharp.Threading.Tasks;
using Unity.Services.Multiplayer;
using Unity.Multiplayer.Widgets;
using System;
using Unity.Services.Lobbies;
using UniRx;
using Unity.Netcode;
using Unity.Services.Relay;
using Unity.Services.Authentication;
using Unity.Services.Core;
using System.Threading.Tasks;

namespace WAK.Managers
{ 

    public class MultiplayManager : PersistentMonoSingleton<MultiplayManager>
    {

        ReactiveProperty<bool> isConnecting = new ReactiveProperty<bool>(false);
        public IReadOnlyReactiveProperty<bool> IsConnecting => isConnecting;
        ReactiveProperty<SessionState> sessionState = new ReactiveProperty<SessionState>(Unity.Services.Multiplayer.SessionState.Disconnected);
        public IReadOnlyReactiveProperty<SessionState> SessionState => sessionState;
        private ISession session;

        protected override void OnInitialized()
        {
            base.OnInitialized();
            InitializeSessionAsync().Forget();
        }

        private void OnSessionAdd(ISession session)
        {
            Debug.Log($"OnSessionAdd : {session}");
        }
        private void OnRemoveSession(ISession session)
        {
            Debug.Log($"OnSessionRemove : {session}");
        }

        private async UniTaskVoid InitializeSessionAsync()
        {

            await UnityServices.InitializeAsync();
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
         

        public async UniTask<ISession> JoinOrJoinSession(string name)
        { 
            ISession session = null;
            try
            {
                isConnecting.Value = true;
                var result = await MultiplayerService.Instance.QuerySessionsAsync(new QuerySessionsOptions()
                {
                    FilterOptions = { new FilterOption(FilterField.Name, name, FilterOperation.Equal) },
                });
                 

                if (result.Sessions.Count > 0)
                {
                    session = await MultiplayerService.Instance.JoinSessionByIdAsync(result.Sessions[0].Id);
                }
                else
                {
                    session = await MultiplayerService.Instance.CreateSessionAsync(new SessionOptions()
                    {
                        Name = name,
                        MaxPlayers = 2,
                        IsPrivate = true,
                        
                    });
                }
                InitializeSession(session);
            }
            finally
            {
                isConnecting.Value = false;
            }

            return session; 
        }

        public async UniTask<ISession> JoinSession(string code)
        {
            ISession session = null;
            try
            {
                isConnecting.Value = true;
                session = await MultiplayerService.Instance.JoinSessionByIdAsync(code);
                InitializeSession(session);
            }
            finally
            {
                isConnecting.Value = false;
            }

            return session; 
        }

        private void InitializeSession(ISession session)
        {
            this.session = session;
            session.SessionPropertiesChanged += OnChangeSessionProperties;
            session.Deleted += OnSessionDeleted;
            session.Changed += OnSessionStateChanged;
            session.PlayerPropertiesChanged += OnPlayerPropertiesChanged;
            session.PlayerJoined += OnPlayerJoined;
            session.PlayerLeft += OnPlayerLeave;

            sessionState.Value = session.State;
            Debug.Log($"Session Init - Id: {session.Id} | Name: {session.Name} | PlayerCount: {session.PlayerCount}");
        }

        private void OnPlayerLeave(string obj)
        {
            Debug.Log($"OnPlayerLeave : {obj} ");
        }

        private void OnPlayerJoined(string obj)
        {
            Debug.Log($"OnPlayerJoined : {obj} ");
        }

        private void OnSessionStateChanged()
        {
            sessionState.Value = session.State;
            Debug.Log($"OnSessionStateChanged : {session.State}");
        }

        private void OnSessionDeleted()
        {
            session = null;
            sessionState.Value = Unity.Services.Multiplayer.SessionState.Deleted;
        }

        private void OnPlayerPropertiesChanged()
        {

        }

        private void OnChangeSessionProperties()
        {

        }
    }
}

