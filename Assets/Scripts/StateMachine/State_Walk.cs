using UnityEngine;

public class State_Walk : IState
{
    private State_Machine_Player stateMachine;
    private Player_Input_Controller plController;
    private Animator animator;
    private string animationName = "Walk";

    public string AnimationName { get => animationName; }

    public State_Walk(State_Machine_Player stateMachine, Player_Input_Controller plController, Animator animator)
    {
        this.stateMachine = stateMachine;
        this.plController = plController;
        this.animator = animator;
    }

    public void OnEnter()
    {
        animator.Play(animationName);
        plController.isMoving = true;
    }

    public void OnExit()
    {
        plController.isMoving = false;
    }

    public void OnUpdate()
    {
        if (plController.isMoving == false)
        {
            stateMachine.ChangeState(typeof(State_Idle));
        }
    }

    public void OnAuto() { }
}
