using UnityEngine;
// !No animation
public class State_Fall : IState
{
    private State_Machine_Player stateMachine;
    private Player_Input_Controller plController;
    private Animator animator;
    private string animationName = "Fall";
    public string AnimationName { get => animationName; }

    public State_Fall(State_Machine_Player stateMachine, Player_Input_Controller plController, Animator animator)
    {
        this.stateMachine = stateMachine;
        this.plController = plController;
        this.animator = animator;
    }

    public void OnEnter()
    {
        animator.Play(animationName);
    }

    public void OnExit() { }

    public void OnUpdate()
    {
        if (plController.isLanded)
        {
            plController.rigidBody.velocity = Vector3.zero;
            stateMachine.ChangeState(typeof(State_Idle));
        }
    }

    public void OnAuto() { }
}
