// PlayerControllerBase.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WAK.Game
{

    public class PlayerControllerBase : MonoBehaviour
    {
        // StateMachine을 제너릭으로 설정
        protected StateMachine<StateBase> InputStateMachine;
        protected StateMachine<StateBase> PlayerStateMachine;

        // Input Action 인스턴스

        // 플레이어 프로퍼티 (예시)
        protected int health = 100;

        protected virtual void Awake()
        {
            InputStateMachine = new StateMachine<StateBase>();
            PlayerStateMachine = new StateMachine<StateBase>();

        }
         
        protected virtual void OnDestroy()
        {
        }

        
        protected virtual void Update()
        {
            InputStateMachine.Update();
            PlayerStateMachine.Update();
        }
         
        public virtual void Initalize()
        {
            InputStateMachine.SwitchState(StateBase.GetOrCreate<InputState_Play>(this));
            PlayerStateMachine.SwitchState(StateBase.GetOrCreate<PlayerState_Wait>(this));
        }
         

    }
}