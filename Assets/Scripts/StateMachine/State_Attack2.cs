using UnityEngine;

public class State_Attack2 : IState
{
    private State_Machine_Player stateMachine;
    private Player_Input_Controller plController;
    private Animator animator;
    private string animationName = "Attack2";

    public string AnimationName { get => animationName; }

    public State_Attack2(State_Machine_Player stateMachine, Player_Input_Controller plController, Animator animator)
    {
        this.stateMachine = stateMachine;
        this.plController = plController;
        this.animator = animator;
    }

    public void OnEnter()
    {
        plController.isAttack = true;
        animator.Play(animationName);
        plController.SetControlActivity(false);
        stateMachine.StartCoroutine(stateMachine.GoShortPeriod(0.15f));
    }

    public void OnExit() { }

    public void OnUpdate()
    {
        plController.rigidBody.velocity = Vector3.zero;
    }

    public void OnAuto()
    {
        stateMachine.ChangeState(typeof(State_Attack3));
    }
}
