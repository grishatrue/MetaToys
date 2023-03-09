using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robot_Controller : MonoBehaviour
{
    [HideInInspector] public Base_Data dataObject;
    [HideInInspector] public BoxCollider boxCollider;
    [HideInInspector] public Rigidbody rigidBody;
    [HideInInspector] public Animator animator;

    public LayerMask mask;
    public Player_Detector_Trigger playerDetector;
    public Player_Detector_Trigger attackTrigger;

    [HideInInspector] public GameObject targetObjet = null;
    [HideInInspector] public Vector3 spawnPoint;
    [HideInInspector] public Vector3 lastRoutePointPos;
    [HideInInspector] public int direction = 1;

    private IState lastState;
    private IState currentState;
    private Dictionary<Type, IState> states;

    private bool isShortPeriod = false;
    private bool isDataObjectAttached = false;

    [HideInInspector] public string propPatSpd;
    [HideInInspector] public string propFollSpd;
    [HideInInspector] public string propHP;
    [HideInInspector] public string propWatingTargetSecs;
    [HideInInspector] public string propDamage;

    public IState LastState { get { return lastState; } }
    public IState CurrentState { get { return currentState; } }
    public Dictionary<Type, IState> States { get { return states; } }
    public bool IsShortPeriod { get { return isShortPeriod; } }
    public bool IsDataObjectAttached { get { return isDataObjectAttached; } }

    private void Awake()
    {
        StatesInit();
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        isDataObjectAttached = TryGetComponent<Base_Data>(out dataObject);
        spawnPoint = transform.position;
        lastRoutePointPos = new Vector3(spawnPoint.x + 1000f, spawnPoint.y, spawnPoint.z);

        if (IsDataObjectAttached)
        {
            dataObject.TryGetProperty("PatrolSpeed", out propPatSpd);
            dataObject.TryGetProperty("FollowingSpeed", out propFollSpd);
            dataObject.TryGetProperty("HitPoints", out propHP);
            dataObject.TryGetProperty("WaitingTheTargetSeconds", out propWatingTargetSecs);
            dataObject.TryGetProperty("Damage", out propDamage);

            ChangeState(typeof(StateRobot_Patrol));
        }
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
            [typeof(StateRobot_Waiting)] = new StateRobot_Waiting(this),
            [typeof(StateRobot_Patrol)] = new StateRobot_Patrol(this),
            [typeof(StateRobot_Following)] = new StateRobot_Following(this),
            [typeof(StateRobot_GoToSpawn)] = new StateRobot_GoToSpawn(this),
            [typeof(StateRobot_Shock)] = new StateRobot_Shock(this),
            [typeof(StateRobot_Attack)] = new StateRobot_Attack(this)
        };
    }

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

    public void StateOnAuto()
    {
        currentState.OnAuto();
    }

    public bool FindRoutPointInBoxArea()
    {
        var bcs = boxCollider.size;
        Collider[] inRes = Physics.OverlapBox(
            transform.position + boxCollider.center, new Vector3(bcs.x, bcs.y, bcs.z),
            Quaternion.identity, mask, QueryTriggerInteraction.Collide);

        for (int i = 0; i < inRes.Length; i++)
        {
            if (inRes[i].TryGetComponent(out Route_Point _))
            {
                return true;
            }
        }

        return false;
    }

    public IEnumerator GoShortPeriod(float seconds)
    {
        isShortPeriod = true;
        yield return new WaitForSeconds(seconds);
        isShortPeriod = false;
    }
}

public class StateRobot_Waiting : IState
{
    private Robot_Controller robotController;
    private Animator animator;
    private string animationName = "Idle";

    public string AnimationName { get => animationName; }

    public StateRobot_Waiting(Robot_Controller robotController)
    {
        this.robotController = robotController;
    }

    public void OnEnter()
    {
        robotController.animator.Play(animationName);
        robotController.StartCoroutine(robotController.GoShortPeriod(float.Parse(robotController.propWatingTargetSecs)));
    }

    public void OnExit() { }

    public void OnUpdate()
    {
        robotController.rigidBody.velocity = new Vector3(0, robotController.rigidBody.velocity.y, 0);

        if (robotController.playerDetector.FindPlayerInBoxArea(out GameObject target))
        {
            robotController.targetObjet = target;

            if (robotController.attackTrigger.FindPlayerInBoxArea(out GameObject _))
            {
                robotController.ChangeState(typeof(StateRobot_Attack));
            }
            else
            {
                robotController.ChangeState(typeof(StateRobot_Following));
            }
        }

        if (!robotController.IsShortPeriod)
        {
            robotController.ChangeState(typeof(StateRobot_Patrol));
        }
    }

    public void OnAuto() { }
}

public class StateRobot_Patrol : IState
{
    private Robot_Controller robotController;
    private string animationName = "Idle";
    private float distPlayer;
    private float distPoint;
    private bool isPointDetected = false;

    public string AnimationName { get => animationName; }

    public StateRobot_Patrol(Robot_Controller robotController)
    {
        this.robotController = robotController;
    }

    public void OnEnter()
    {
        robotController.animator.Play(animationName);
        distPlayer = Mathf.Abs(robotController.spawnPoint.x - robotController.transform.position.x);
        distPoint = Mathf.Abs(robotController.spawnPoint.x - robotController.lastRoutePointPos.x);

        if (Mathf.Abs(distPlayer) > Mathf.Abs(distPoint))
        {
            robotController.ChangeState(typeof(StateRobot_GoToSpawn));
        }
    }
    public void OnExit() { }
    public void OnUpdate()
    {
        if (robotController.FindRoutPointInBoxArea())
        {
            if (!isPointDetected)
            {
                isPointDetected = true;
                robotController.direction = (int)new Vector2(robotController.spawnPoint.x - robotController.transform.position.x, 0).normalized.x;
                robotController.transform.rotation = new Quaternion(0, robotController.direction == 1 ? 180 : 0, 0, 0);
            }
        }
        else
        {
            isPointDetected = false;
        }

        robotController.rigidBody.velocity = new Vector3(
            float.Parse(robotController.propPatSpd) * robotController.direction, robotController.rigidBody.velocity.y, 0);

        if (robotController.playerDetector.FindPlayerInBoxArea(out GameObject target))
        {
            robotController.targetObjet = target;
            robotController.ChangeState(typeof(StateRobot_Following));
        }
    }
    public void OnAuto() { }
}

public class StateRobot_Following : IState
{
    private Robot_Controller robotController;
    private string animationName = "Run";

    public string AnimationName { get => animationName; }

    public StateRobot_Following(Robot_Controller robotController)
    {
        this.robotController = robotController;
    }

    public void OnEnter()
    {
        robotController.animator.Play(animationName);
    }

    public void OnExit() { }

    public void OnUpdate()
    {
        if (robotController.FindRoutPointInBoxArea())
        {
            robotController.lastRoutePointPos = robotController.transform.position;
        }

        if (robotController.playerDetector.FindPlayerInBoxArea(out GameObject _))
        {
            var targetPosX = robotController.targetObjet.transform.position.x;
            robotController.direction = (int)new Vector2(targetPosX - robotController.transform.position.x, 0).normalized.x;
            robotController.rigidBody.velocity =
                new Vector3(float.Parse(robotController.propFollSpd) * robotController.direction, robotController.rigidBody.velocity.y, 0);
            robotController.transform.rotation = new Quaternion(0, robotController.direction == 1 ? 180 : 0, 0, 0);
        }
        else
        {
            robotController.targetObjet = null;
            robotController.ChangeState(typeof(StateRobot_Waiting));
        }

        if (robotController.attackTrigger.FindPlayerInBoxArea(out GameObject _))
        {
            robotController.ChangeState(typeof(StateRobot_Attack));
        }
    }
     
    public void OnAuto() { }
}

public class StateRobot_Shock : IState
{
    private Robot_Controller robotController;
    private string animationName = "Shock";

    public string AnimationName { get => animationName; }

    public StateRobot_Shock(Robot_Controller robotController)
    {
        this.robotController = robotController;
    }

    public void OnEnter()
    {
        robotController.animator.Play(animationName);
    }

    public void OnExit() { }

    public void OnUpdate() { }

    public void OnAuto()
    {
        robotController.ChangeState(typeof(StateRobot_Patrol));
    }
}

public class StateRobot_Attack : IState
{
    private Robot_Controller robotController;
    private string animationName = "Attack";
    private int onAutoCounter = 0;

    public string AnimationName { get { return animationName; } }

    public StateRobot_Attack(Robot_Controller robotController)
    {
        this.robotController = robotController;
    }

    public void OnEnter()
    {
        robotController.animator.Play(animationName);
        onAutoCounter = 0;
    }

    public void OnExit()
    {
        onAutoCounter = 0;
    }

    public void OnUpdate()
    {
        robotController.rigidBody.velocity = new Vector3(0, robotController.rigidBody.velocity.y, 0);
    }

    public void OnAuto()
    {
        if (onAutoCounter == 0)
        {
            if (robotController.attackTrigger.FindPlayerInBoxArea(out GameObject _))
            {
                robotController.targetObjet.GetComponent<Input_Data>().GetDamage(int.Parse(robotController.propDamage));
            }
        }

        if (onAutoCounter == 1)
        {
            if (robotController.playerDetector.FindPlayerInBoxArea(out GameObject _))
            {
                if (robotController.attackTrigger.FindPlayerInBoxArea(out GameObject _))
                {
                    robotController.ChangeState(typeof(StateRobot_Attack));
                    onAutoCounter -= 1;
                }
                else
                {
                    robotController.ChangeState(typeof(StateRobot_Following));
                }
            }
            else
            {
                robotController.ChangeState(typeof(StateRobot_Waiting));
            }
        }

        onAutoCounter += 1;
    }
}

public class StateRobot_GoToSpawn : IState
{
    private Robot_Controller robotController;
    private string animationName = "Idle";

    public string AnimationName { get => animationName; }

    public StateRobot_GoToSpawn(Robot_Controller robotController)
    {
        this.robotController = robotController;
    }

    public void OnEnter()
    {
        robotController.direction = (int)new Vector2(robotController.spawnPoint.x - robotController.transform.position.x, 0).normalized.x;
        robotController.transform.rotation = new Quaternion(0, robotController.direction == 1 ? 180 : 0, 0, 0);
    }
    public void OnExit() { }
    public void OnUpdate()
    {
        robotController.rigidBody.velocity = new Vector3(
            float.Parse(robotController.propPatSpd) * robotController.direction, robotController.rigidBody.velocity.y, 0);

        if (robotController.FindRoutPointInBoxArea())
        {
            robotController.ChangeState(typeof(StateRobot_Patrol));
        }

        if (robotController.playerDetector.FindPlayerInBoxArea(out GameObject target))
        {
            robotController.targetObjet = target;
            robotController.ChangeState(typeof(StateRobot_Following));
        }
    }
    public void OnAuto() { }
}