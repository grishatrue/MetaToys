using UnityEngine;

public class State_Jump : IState
{
    private State_Machine_Player stateMachine;
    private Player_Input_Controller plController;
    private Animator animator;
    private string animationName = "Jump";
    private bool flag = true;

    public string AnimationName { get => animationName; }

    public State_Jump(State_Machine_Player stateMachine, Player_Input_Controller plController, Animator animator)
    {
        this.stateMachine = stateMachine;
        this.plController = plController;
        this.animator = animator;
    }

    public void OnEnter()
    {
        if (stateMachine.LastState == stateMachine.CurrentState)
        {
            animator.Play(stateMachine.States[typeof(State_Idle)].AnimationName);
        }
        else
        {
            animator.Play(animationName);
        }
    }

    public void OnExit()
    {
        flag = true;
    }

    public void OnUpdate()
    {
        if (flag)
        {
            flag = false;
            animator.Play(animationName);
        }

        if (plController.isLanded)
        {
            stateMachine.ChangeState(typeof(State_Idle));
        }
    }

    public void OnAuto()
    {
        stateMachine.ChangeState(typeof(State_Fall));
    }
}
