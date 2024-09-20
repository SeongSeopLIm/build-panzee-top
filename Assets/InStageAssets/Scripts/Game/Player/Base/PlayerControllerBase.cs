// PlayerControllerBase.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace WAK.Game
{

    public class PlayerControllerBase : MonoBehaviour
    {
        public StateMachine<StateBase> InputStateMachine { get; protected set; } = new();
        // �ϴ� ����� �ߴµ� �ʿ���� �� ���Ƽ� �ӽ� �ּ�ó��. ���Ѻ��ٰ� �� ���� ����.
        //public StateMachine<StateBase> PlayerStateMachine { get; protected set; } = new();

         
         
        protected virtual void OnDestroy()
        {
        }

        
        protected virtual void Update()
        {
            InputStateMachine.Update(); 
        }
         
        public virtual void Initalize()
        { 

        } 
    }
}