using UnityEngine;

public class State_SpecialAttack : IState
{
    private State_Machine_Player stateMachine;
    private Player_Input_Controller plController;
    private Animator animator;
    private string animationName = "AttackSpecial";
    private float yPos = 0;

    public string AnimationName { get => animationName; }

    public State_SpecialAttack(State_Machine_Player stateMachine, Player_Input_Controller plController, Animator animator)
    {
        this.stateMachine = stateMachine;
        this.plController = plController;
        this.animator = animator;
    }

    public void OnEnter()
    {
        animator.Play(animationName);
        plController.SetControlActivity(false);
        yPos = plController.transform.position.y;
    }

    public void OnExit()
    {
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
