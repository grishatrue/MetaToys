using UnityEngine;

public class State_Idle : IState
{
    private State_Machine_Player stateMachine;
    private Player_Input_Controller plController;
    private Animator animator;
    private string animationName = "Idle";

    public string AnimationName { get => animationName; }

    public State_Idle(State_Machine_Player stateMachine, Player_Input_Controller plController, Animator animator)
    {
        this.stateMachine = stateMachine;
        this.plController = plController;
        this.animator = animator;
    }

    public void OnEnter()
    {
        if (stateMachine.LastState != stateMachine.CurrentState)
        {
            animator.Play(animationName);
        }
    }

    public void OnExit() { }

    public void OnUpdate()
    {
        if (!plController.isLanded)
        {
            stateMachine.ChangeState(typeof(State_Fall));
        }
    }

    public void OnAuto() { }
}
