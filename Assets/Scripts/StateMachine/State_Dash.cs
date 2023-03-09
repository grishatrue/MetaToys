using UnityEngine;
// !No animation. This state needs to be controlled by a flag
public class State_Dash : IState
{
    private State_Machine_Player stateMachine;
    private Player_Input_Controller plController;
    private Animator animator;
    private string animationName = "Dash";
    private float yPos = 0;
    public string AnimationName { get => animationName; }

    public State_Dash(State_Machine_Player stateMachine, Player_Input_Controller plController, Animator animator)
    {
        this.stateMachine = stateMachine;
        this.plController = plController;
        this.animator = animator;
    }

    public void OnEnter()
    {
        animator.Play(animationName);
        plController.isDashing = true;
        yPos = plController.transform.position.y;
    }

    public void OnExit()
    {
        plController.isDashing = false;
        plController.rigidBody.velocity = Vector3.zero;
    }

    public void OnUpdate()
    {
        var pos = plController.transform.position;
        var vel = plController.rigidBody.velocity;
        plController.transform.position = new Vector3(pos.x, yPos, pos.z);
        plController.rigidBody.velocity = new Vector3(vel.x, 0, vel.z);
    }

    public void OnAuto()
    {
        if (plController.isLanded)
        {
            stateMachine.ChangeState(typeof(State_Idle));
        }
        else
        {
            stateMachine.ChangeState(typeof(State_Fall));
        }
    }
}
