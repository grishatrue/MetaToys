using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Input_Controller : MonoBehaviour
{
    private Base_Data dataObject;
    public BoxCollider downSide;
    [HideInInspector] public Rigidbody rigidBody;
    private State_Machine_Player stateMachine;
    public GameObject attack1Trigger;
    public GameObject attackSpecialTrigger;

    private bool isDataObjectAttached = false;
    private bool isControlActive = true;
    [HideInInspector] public bool isLanded = false;
    [HideInInspector] public bool isDashing = false;
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public bool isAttack = false;

    private int propDamage1;
    private int propDamage2;
    private int propDamageSpecial;
    private int propJumpNumber;
    [HideInInspector] public int jumpCounter = 0;

    [HideInInspector] public Dictionary<CtrlKeys, bool> buttonActivitySwitches = new Dictionary<CtrlKeys, bool>();
    private Dictionary<CtrlKeys, bool> LastTruebuttonActivitySwitches = new Dictionary<CtrlKeys, bool>();
    private CtrlKeys lockedButtKey = new CtrlKeys();

    [HideInInspector] public Dictionary<CtrlKeys, KeyCode> ctrlKeyCodes = new Dictionary<CtrlKeys, KeyCode> 
    { 
        [CtrlKeys.LEFT] = KeyCode.A,
        [CtrlKeys.RIGHT] = KeyCode.D,
        [CtrlKeys.JUMP] = KeyCode.W,
        [CtrlKeys.DASH] = KeyCode.K,
        [CtrlKeys.HAND_KICK] = KeyCode.L,
        [CtrlKeys.SP_ATTACK] = KeyCode.O,
    };

    public bool IsControlActive { get { return isControlActive; } }
    public int JumpNumber { get { return propJumpNumber; } }

    private void Awake()
    {
        isDataObjectAttached = TryGetComponent<Base_Data>(out dataObject);

        if (isDataObjectAttached)
        {
            PropInit();
        }
    }

    private void Start()
    {
        Finish.OnLevelFinished += OnLevelFinish;
        Dead_Event.OnPlayerDied += OnPlayerDie;
        rigidBody = GetComponent<Rigidbody>();
        stateMachine = GetComponent<State_Machine_Player>();
        
        foreach (var i in ctrlKeyCodes)
        {
            buttonActivitySwitches.Add(i.Key, isControlActive);
        }
    }

    private void Update()
    {
        if (isDataObjectAttached)
        {
            if (isControlActive)
            {
                OnPropWalk();
                OnPropJump();
                OnPropDash();
                OnPropDamage();
                OnPropDamageSpecial();
            }
        }
    }

    private void OnPropWalk()
    {
        if (dataObject.TryGetProperty("WalkSpeed", out string ws))
        {
            if (buttonActivitySwitches[CtrlKeys.LEFT] && buttonActivitySwitches[CtrlKeys.RIGHT])
            {
                if (!isDashing && !isAttack)
                {
                    float inpHor = Input.GetAxis("Horizontal");

                    if (inpHor != 0)
                    {
                        isMoving = true;
                        int walkSpeed = int.Parse(ws);

                        if (!isAttack)
                        {
                            if (inpHor > 0) // заменить на кнопу из массива // qqq // err
                            {
                                this.gameObject.transform.rotation = new Quaternion(0, 180, 0, 0);
                            }

                            if (inpHor < 0)
                            {
                                this.gameObject.transform.rotation = new Quaternion(0, 0, 0, 0);
                            }
                        }

                        rigidBody.velocity = new Vector3(Input.GetAxis("Horizontal") * walkSpeed, rigidBody.velocity.y, 0);

                        if (isLanded)
                        {
                            if (stateMachine.CurrentState != stateMachine.States[typeof(State_Walk)])
                            {
                                stateMachine.ChangeState(typeof(State_Walk));
                            }
                        }
                    }
                    else
                    {
                        isMoving = false;
                    }
                }
            }
        }
    }

    private void OnPropJump()
    {
        if (dataObject.TryGetProperty("JumpForce", out string jf))
        {
            if (buttonActivitySwitches[CtrlKeys.JUMP])
            {
                if (Input.GetKeyDown(ctrlKeyCodes[CtrlKeys.JUMP]) && jumpCounter > 0 && !isDashing)
                {
                    rigidBody.velocity = new Vector2() { x = rigidBody.velocity.x, y = 0f };
                    jumpCounter = jumpCounter - 1;
                    int jumpForce = int.Parse(jf);
                    rigidBody.AddForce(new Vector3(0, jumpForce, 0));
                    stateMachine.ChangeState(typeof(State_Jump));
                }
            }
        }
    }

    private void OnPropDash()
    {
        if (dataObject.TryGetProperty("DashReloadTime", out string drt))
        {
            if (buttonActivitySwitches[CtrlKeys.DASH])
            {
                if (Input.GetKey(ctrlKeyCodes[CtrlKeys.DASH]) && isLanded)
                {
                    StartCoroutine(ButtDalay(float.Parse(drt), CtrlKeys.DASH));
                    stateMachine.ChangeState(typeof(State_Dash));
                    float direction = gameObject.transform.rotation.y == 0 ? -1 : 1;

                    if (dataObject.TryGetProperty("WalkSpeed", out string ws))
                    {
                        rigidBody.velocity = new Vector3(direction * int.Parse(ws) * 5, 0, 0);
                    }
                }
            }
        }
    }

    private void OnPropDamage()
    {
        if (buttonActivitySwitches[CtrlKeys.HAND_KICK])
        {
            if (Input.GetKey(ctrlKeyCodes[CtrlKeys.HAND_KICK]) && !isAttack)
            {
                stateMachine.ChangeState(typeof(State_Attack1));
            }
        }
    }

    private void OnPropDamageSpecial()
    {
        if (buttonActivitySwitches[CtrlKeys.SP_ATTACK])
        {
            if (Input.GetKey(ctrlKeyCodes[CtrlKeys.SP_ATTACK]) && !isAttack)
            {
                stateMachine.ChangeState(typeof(State_SpecialAttack));
            }
        }
    }

    // // // ########################### ############################
    // // // ########################### \_(`-`)_/    ->    [_(-` )_]

    public void PropInit()
    {
        propJumpNumber = dataObject.TryGetProperty("JumpNumber", out string jn) ? int.Parse(jn) : new int();
        propDamage1 = dataObject.TryGetProperty("Damage1", out string d1) ? int.Parse(d1) : new int();
        propDamage2 = dataObject.TryGetProperty("Damage2", out string d2) ? int.Parse(d2) : new int();
        propDamageSpecial = dataObject.TryGetProperty("DamageSpecial", out string ds) ? int.Parse(ds) : new int();
    }

    public void SetDamage() // вызов от animation event
    {
        int damage = 0;
        if (stateMachine.CurrentState == stateMachine.States[typeof(State_Attack1)]) { damage = propDamage1; }
        if (stateMachine.CurrentState == stateMachine.States[typeof(State_Attack2)]) { damage = propDamage2; }
        List<GameObject> enemies = attack1Trigger.GetComponent<Attack_Trigger>().FindEnemyInBoxArea();
        
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].GetComponent<Input_Data>().GetDamage(damage);
        }
    }

    public void SetDamageSpecial() // вызов от animation event
    {
        int damage = propDamageSpecial;
        List<GameObject> enemies = attackSpecialTrigger.GetComponent<Attack_Trigger>().FindEnemyInBoxArea();
        foreach (GameObject i in enemies)
        {
            i.GetComponent<Input_Data>().GetDamage(damage);
        }
    }

    public void SetExploseSpecial() // вызов от animation event
    {
        List<GameObject> enemies = attackSpecialTrigger.GetComponent<Attack_Trigger>().FindEnemyInBoxArea();

        foreach (var i in enemies)
        {
            var dir = new Vector2(i.transform.position.x - transform.position.x, 0).normalized.x;
            i.GetComponent<Rigidbody>().AddForce(dir * 320, 320, 0);
        }
    }

    public void SetControlActivity(bool active)
    {
        StopCoroutine(ButtDalay(0f, new CtrlKeys()));
        if (active) { LastTruebuttonActivitySwitches = buttonActivitySwitches; }
        List< CtrlKeys> keys = new List<CtrlKeys>();
        foreach (var i in buttonActivitySwitches) { keys.Add(i.Key); }
        buttonActivitySwitches.Clear();
        foreach (var i in keys) { buttonActivitySwitches.Add(i, active); }
        isControlActive = active;
    }

    public void SetControlActivity(bool active, CtrlKeys controlButtonKey)
    {
        if (lockedButtKey != new CtrlKeys())
        {
            StopCoroutine(ButtDalay(0f, new CtrlKeys()));
        }

        buttonActivitySwitches[controlButtonKey] = active;
        LastTruebuttonActivitySwitches = buttonActivitySwitches;
        CheckControlActivity();
    }

    public void SetLastTrueControlActivity()
    {
        buttonActivitySwitches = LastTruebuttonActivitySwitches;
    }

    private void CheckControlActivity()
    {
        isControlActive = false;

        foreach (var i in buttonActivitySwitches)
        {
            if (i.Value == true)
            {
                isControlActive = true;
                break;
            }
        }
    }

    public IEnumerator ShortJerk(float time)
    {
        if (dataObject.TryGetProperty("WalkSpeed", out string ws))
        {
            int direction = transform.rotation.y == 0 ? -1 : 1;
            rigidBody.velocity = new Vector3(direction * int.Parse(ws) * 0.75f, rigidBody.velocity.y);
            yield return new WaitForSeconds(time);
            rigidBody.velocity = new Vector3(0, rigidBody.velocity.y);
        }
    }

    private IEnumerator ButtDalay(float lockTime, CtrlKeys buttonKeyName)
    {
        buttonActivitySwitches[buttonKeyName] = false;
        lockedButtKey = buttonKeyName;
        yield return new WaitForSeconds(lockTime);
        buttonActivitySwitches[buttonKeyName] = true;
        lockedButtKey = new CtrlKeys();
    }

    private void OnLevelFinish()
    {
        Finish.OnLevelFinished -= OnLevelFinish;
        Dead_Event.OnPlayerDied -= OnPlayerDie;
        isControlActive = false;
        rigidBody.velocity = Vector3.zero;
    }

    private void OnPlayerDie()
    {
        Finish.OnLevelFinished -= OnLevelFinish;
        Dead_Event.OnPlayerDied -= OnPlayerDie;
        isControlActive = false;
        // GetComponent<State_Machine>().ChangeState("dead");
           Destroy(this.gameObject, 0.5f); // перенести в State_Machine
    }
}
