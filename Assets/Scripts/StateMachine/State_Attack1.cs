using UnityEngine;

public class State_Attack1 : IState
{
    private State_Machine_Player stateMachine;
    private Player_Input_Controller plController;
    private Animator animator;
    private string animationName = "Attack1";
    private bool isComboExtended = false;
    private bool flag = true;

    public string AnimationName { get => animationName; }

    public State_Attack1(State_Machine_Player stateMachine, Player_Input_Controller plController, Animator animator)
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
        stateMachine.StartCoroutine(plController.ShortJerk(0.1f));
    }

    public void OnExit()
    {
        flag = true;
        isComboExtended = false;
        plController.isAttack = false;
        plController.SetControlActivity(true);
    }

    public void OnUpdate()
    {
        if (!stateMachine.IsShortPeriod)
        {
            if (flag)
            {
                if (Input.GetKey(plController.ctrlKeyCodes[CtrlKeys.HAND_KICK]))
                {
                    flag = false;
                    isComboExtended = true;
                }
            }
        }
    }

    public void OnAuto()
    {
        if (isComboExtended)
        {
            stateMachine.ChangeState(typeof(State_Attack2));
        }
        else
        {
            stateMachine.ChangeState(typeof(State_Idle));
        }
    }
}
