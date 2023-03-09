using UnityEngine;

public class State_Attack3 : IState
{
    private State_Machine_Player stateMachine;
    private Player_Input_Controller plController;
    private Animator animator;
    private string animationName = "Attack3";

    public string AnimationName { get => animationName; }

    public State_Attack3(State_Machine_Player stateMachine, Player_Input_Controller plController, Animator animator)
    {
        this.stateMachine = stateMachine;
        this.plController = plController;
        this.animator = animator;
    }

    public void OnEnter()
    {
        animator.Play(animationName);
        stateMachine.StartCoroutine(stateMachine.GoShortPeriod(0.15f));
    }

    public void OnExit()
    {
        plController.isAttack = false;
        plController.SetControlActivity(true);
    }

    public void OnUpdate()
    {
        plController.rigidBody.velocity = Vector3.zero;
    }

    public void OnAuto()
    {
        stateMachine.ChangeState(typeof(State_Idle));
    }
}
