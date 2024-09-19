// PlayerControllerBase.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WAK.Game
{

    public class PlayerControllerBase : MonoBehaviour
    {
        // StateMachine�� ���ʸ����� ����
        protected StateMachine<StateBase> InputStateMachine;
        protected StateMachine<StateBase> PlayerStateMachine;

        // Input Action �ν��Ͻ�

        // �÷��̾� ������Ƽ (����)
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