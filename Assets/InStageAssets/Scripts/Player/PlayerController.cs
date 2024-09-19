// PlayerControllerBase.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class PlayerControllerBase : MonoBehaviour
{
    // StateMachine�� ���ʸ����� ����
    protected StateMachine<StateBase> InputStateMachine;
    protected StateMachine<StateBase> PlayerStateMachine;

    // Input Action �ν��Ͻ�
    protected MainControl mainControl;

    // �÷��̾� ������Ƽ (����)
    protected int health = 100;

    protected virtual void Awake()
    {
        // MainControl �ν��Ͻ� ���� �� �ݹ� ����
        mainControl = new MainControl();
        // InputStateMachine�� PlayerInputState�� ����
        InputStateMachine = new StateMachine<StateBase>();
        // PlayerStateMachine�� PlayerState�� ����
        PlayerStateMachine = new StateMachine<StateBase>();

        // �ʱ� ���� ����
        InitializeStates();
    }

    /// <summary>
    /// ���� �ʱ�ȭ�� ���� �޼���
    /// ��ӹ޴� Ŭ�������� ����
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
        // ���� �ӽ� ������Ʈ ȣ��
        InputStateMachine.Update();
        PlayerStateMachine.Update();
    }

    /// <summary>
    /// �÷��̾��� ü���� �����ϴ� �޼���
    /// </summary>
    /// <param name="health">���ο� ü�� ��</param>
    public virtual void SetPlayerHealth(int health)
    {
        this.health = health;
        Debug.Log($"Player Health set to {health}");
        // ���� ���� ������ ���� ü�� ���� ����
    }

    /// <summary>
    /// IPlayActions �������̽� ����: tap �׼ǿ� ���� �ݹ�
    /// </summary>
    /// <param name="context">�Է� �ݹ� ���ؽ�Ʈ</param>
    public virtual void OnTap(InputAction.CallbackContext context)
    {
        // �⺻ ����: �ƹ� ���۵� ���� ����
    }

    /// <summary>
    /// tap �׼� ó�� �޼���
    /// ��ӹ޴� Ŭ�������� ����
    /// </summary>
    protected abstract void HandleTapAction();
}