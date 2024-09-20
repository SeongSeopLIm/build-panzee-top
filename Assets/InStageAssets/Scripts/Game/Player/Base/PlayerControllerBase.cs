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
        // 일단 만들긴 했는데 필요없는 거 같아서 임시 주석처리. 지켜보다가 안 쓰면 삭제.
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