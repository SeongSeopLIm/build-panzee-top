// PlayerControllerBase.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerControllerBase : MonoBehaviour
{
    // StateMachine을 제너릭으로 설정
    protected StateMachine<StateBase> InputStateMachine;
    protected StateMachine<StateBase> PlayerStateMachine;

    // Input Action 인스턴스
    protected MainControl mainControl;

    // 플레이어 프로퍼티 (예시)
    protected int health = 100;

    protected virtual void Awake()
    {
        // MainControl 인스턴스 생성 및 콜백 설정
        mainControl = new MainControl();
        // InputStateMachine은 PlayerInputState를 관리
        InputStateMachine = new StateMachine<StateBase>();
        // PlayerStateMachine은 PlayerState를 관리
        PlayerStateMachine = new StateMachine<StateBase>();

        // 초기 상태 설정
        InitializeStates();
    }

    /// <summary>
    /// 상태 초기화를 위한 메서드
    /// 상속받는 클래스에서 구현
    /// </summary>
    protected abstract void InitializeStates();

    protected virtual void OnDestroy()
    {
        if (mainControl != null)
        {
            mainControl.Dispose();
        }
    }

    protected virtual void Update()
    {
        // 상태 머신 업데이트 호출
        InputStateMachine.Update();
        PlayerStateMachine.Update();
    }

    /// <summary>
    /// 플레이어의 체력을 설정하는 메서드
    /// </summary>
    /// <param name="health">새로운 체력 값</param>
    public virtual void SetPlayerHealth(int health)
    {
        this.health = health;
        Debug.Log($"Player Health set to {health}");
        // 실제 게임 로직에 따라 체력 변경 적용
    }

    /// <summary>
    /// IPlayActions 인터페이스 구현: tap 액션에 대한 콜백
    /// </summary>
    /// <param name="context">입력 콜백 컨텍스트</param>
    public virtual void OnTap(InputAction.CallbackContext context)
    {
        // 기본 구현: 아무 동작도 하지 않음
    }

    /// <summary>
    /// tap 액션 처리 메서드
    /// 상속받는 클래스에서 구현
    /// </summary>
    protected abstract void HandleTapAction();
}