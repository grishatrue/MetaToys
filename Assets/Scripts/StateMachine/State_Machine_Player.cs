using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Machine_Player : MonoBehaviour
{
    private IState currentState;
    private IState lastState;
    private Dictionary<Type, IState> states;

    private Animator animator;
    private Player_Input_Controller plController;
    private bool isShortPeriod = false;

    public IState CurrentState { get { return currentState; } }
    public IState LastState { get { return lastState; } }
    public Dictionary<Type, IState> States { get { return states; } }
    public bool IsShortPeriod { get { return isShortPeriod; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        plController = GetComponent<Player_Input_Controller>();
        StatesInit();
    }

    private void Start()
    {
        ChangeState(typeof(State_Idle));
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    private void StatesInit()
    {
        states = new Dictionary<Type, IState>()
        {
            [typeof(State_Idle)] = new State_Idle(this, plController, animator),
            [typeof(State_Walk)] = new State_Walk(this, plController, animator),
            [typeof(State_Jump)] = new State_Jump(this, plController, animator),
            [typeof(State_Fall)] = new State_Fall(this, plController, animator),
            [typeof(State_Dash)] = new State_Dash(this, plController, animator),
            [typeof(State_Attack1)] = new State_Attack1(this, plController, animator),
            [typeof(State_Attack2)] = new State_Attack2(this, plController, animator),
            [typeof(State_Attack3)] = new State_Attack3(this, plController, animator),
            [typeof(State_SpecialAttack)] = new State_SpecialAttack(this, plController, animator)
        };
    }

    /// <summary>
    /// How to use:<c> ChangeState(typeof( <see cref="IState"></see> )); </c>
    /// </summary>
    public void ChangeState(Type newState)
    {
        if (currentState != null)
        {
            lastState = currentState;
            currentState.OnExit();
        }

        currentState = states[newState];
        currentState.OnEnter();
    }
    
    public void ChangeStateAuto()
    {
        currentState.OnAuto();
    }

    public IEnumerator GoShortPeriod(float seconds)
    {
        isShortPeriod = true;
        yield return new WaitForSeconds(seconds);
        isShortPeriod = false;
    }
}
