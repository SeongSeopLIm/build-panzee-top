using System.Collections.Generic;
using System;
using UnityEngine;

namespace WAK.Game
{

    public abstract class StateBase : IState
    {
        protected PlayerControllerBase playerController;

        public StateBase(PlayerControllerBase controller)
        {
            playerController = controller;
        }

        public virtual void Enter() { }
        public virtual void Exit() { }
        public virtual void Update() { } // �ʿ� ���� �� ������ �ϴ� �߰�.


        public virtual void OnGet() { }
        public virtual void OnRelease() { }


        private static Dictionary<string, StateBase> statesByName = new();
        public static StateBase GetOrCreate<T2>(PlayerControllerBase controller) where T2 : StateBase
        {
            string key = typeof(T2).Name;
            if (statesByName.TryGetValue(key, out var state))
            {
                return state;
            }

            // �����ս��� �̽� ���� �׶� Init ���� ȣ���ϴ� ���·� ��ȯ
            var newState = Activator.CreateInstance(typeof(T2), controller) as StateBase;
            if (newState == null)
            {
                throw new InvalidOperationException($"Cannot create instance of {typeof(T2).Name}");
            }

            statesByName.Add(key, newState);
            return newState;
        }
    }
}