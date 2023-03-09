using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ad_Controller : MonoBehaviour
{
    private Base_Data dataObject;
    [HideInInspector] public Rigidbody rigidBody;

    public LayerMask mask;
    public GameObject bulletPrefab;

    [HideInInspector] public GameObject targetObjet = null;
    [HideInInspector] public Vector3 spawnPoint;
    [HideInInspector] public float speed;
    [HideInInspector] public float visionDistance;

    private IState lastState;
    private IState currentState;
    private Dictionary<Type, IState> states;
    private bool isShortPeriod = false;
    private bool isDataObjectAttached = false;
    [HideInInspector] public float direction = 1;

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
        isDataObjectAttached = TryGetComponent<Base_Data>(out dataObject);

        if (isDataObjectAttached)
        {
            if (dataObject.TryGetProperty("VisionDistance", out string vd)) 
            { 
                visionDistance = float.Parse(vd); 
            }

            if (dataObject.TryGetProperty("Speed", out string hs))
            { 
                speed = float.Parse(hs); 
            }
        }

        spawnPoint = transform.position;
        targetObjet = GameObject.FindGameObjectWithTag("EnemyAggressor");
        ChangeState(typeof(StateAd_Idle));
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
            [typeof(StateAd_Idle)] = new StateAd_Idle(this),
            [typeof(StateAd_Following)] = new StateAd_Following(this),
            [typeof(StateAd_Attack)] = new StateAd_Attack(this),
            [typeof(StateAd_GoingToSpawn)] = new StateAd_GoingToSpawn(this)
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

    public void ChangeStateAuto()
    {
        currentState.OnAuto();
    }

    public IEnumerator GoShortPeriod(float seconds) // нам не нужы корутины, ведь есть асинхрон. Но пока пусть побудет
    {
        isShortPeriod = true;
        yield return new WaitForSeconds(seconds);
        isShortPeriod = false;
    }

    public void AttackBullet()
    {
        GameObject newBullet = Instantiate(bulletPrefab, this.transform.position, Quaternion.identity);
        newBullet.GetComponent<Ad_Bullet_Behavior>().parantCollider = this.GetComponent<Collider>();
        newBullet.transform.LookAt(targetObjet.GetComponent<Aiming_Point>().Center);
    }
    /// <summary>
    /// Param "<paramref name="direction"/>" is -1 or 1 int value
    /// </summary>
    public void SetDirectionAndRotation(float direction)
    {
        this.direction = direction;
        var rot = transform.rotation;
        transform.rotation = new Quaternion(rot.x, direction == 1 ? 0 : 180, rot.z, rot.w);
    }
}

public class StateAd_Idle : IState
{
    private Ad_Controller adController;
    private string animationName = "Idle";

    public string AnimationName { get => animationName; }

    public StateAd_Idle(Ad_Controller adController)
    {
        this.adController = adController;
    }

    public void OnAuto() { }
    public void OnEnter()
    {
        adController.rigidBody.velocity = Vector3.zero;
    }
    public void OnExit() { }
    public void OnUpdate()
    {
        if (adController.targetObjet != null)
        {
            if (Physics.Linecast(adController.transform.position, adController.targetObjet.GetComponent<Aiming_Point>().Center,
                out RaycastHit hitInfo, adController.mask, QueryTriggerInteraction.Ignore))
            {
                if (hitInfo.collider.name == adController.targetObjet.name)
                {
                    if (hitInfo.distance <= adController.visionDistance)
                    {
                        adController.ChangeState(typeof(StateAd_Following));
                    }
                }
            }
        }
    }
}

public class StateAd_Following : IState
{
    private Ad_Controller adController;
    private string animationName = "Walk";

    public string AnimationName { get => animationName; }

    public StateAd_Following(Ad_Controller adController)
    {
        this.adController = adController;
    }

    public void OnAuto() { }
    public void OnEnter()
    {
        adController.StartCoroutine(adController.GoShortPeriod(1f));
    }
    public void OnExit() { }
    public void OnUpdate()
    {
        if (adController.targetObjet != null)
        {
            if (!adController.IsShortPeriod)
            {
                adController.ChangeState(typeof(StateAd_Attack));
            }

            if (Physics.Linecast(adController.transform.position, adController.targetObjet.GetComponent<Aiming_Point>().Center,
                out RaycastHit hitInfo, adController.mask, QueryTriggerInteraction.Ignore))
            {
                if (hitInfo.collider.name == adController.targetObjet.name)
                {
                    if (hitInfo.distance > adController.visionDistance)
                    {
                        adController.ChangeState(typeof(StateAd_GoingToSpawn));
                    }
                }
                else
                {
                    adController.ChangeState(typeof(StateAd_GoingToSpawn));
                }
            }

            var targetPosition = adController.targetObjet.GetComponent<Aiming_Point>().Center;
            adController.SetDirectionAndRotation(new Vector2(targetPosition.x - adController.transform.position.x, 0).normalized.x);

            if (Vector3.Distance(targetPosition, adController.transform.position) > 5) // дистанция не меньше
            {
                adController.rigidBody.velocity = new Vector3(adController.speed * adController.direction, adController.rigidBody.velocity.y, 0);
            }
            else if (Vector3.Distance(targetPosition, adController.transform.position) < 4)
            {
                adController.rigidBody.velocity = new Vector3(adController.speed * (-adController.direction), adController.rigidBody.velocity.y, 0);
            }
        }
        else
        {
            adController.ChangeState(typeof(StateAd_Idle));
        }
    }
}

public class StateAd_Attack : IState
{
    private Ad_Controller adController;
    private string animationName = "Attack";

    public string AnimationName { get => animationName; }

    public StateAd_Attack(Ad_Controller adController)
    {
        this.adController = adController;
    }

    public void OnAuto() { } // err // qqq // Instant bullet here
    public void OnEnter()
    {
        adController.AttackBullet();
        adController.StartCoroutine(adController.GoShortPeriod(0.5f));
    }
    public void OnExit() { }
    public void OnUpdate()
    {
        if (!adController.IsShortPeriod)
        {
            adController.ChangeState(typeof(StateAd_Following));
        }
    }
}

public class StateAd_GoingToSpawn : IState
{
    private Ad_Controller adController;
    private string animationName = "Walk";

    public string AnimationName { get => animationName; }

    public StateAd_GoingToSpawn(Ad_Controller adController)
    {
        this.adController = adController;
    }

    public void OnAuto() { }
    public void OnEnter() { }
    public void OnExit() { }
    public void OnUpdate()
    {
        if (adController.targetObjet != null)
        {
            if (!Physics.Linecast(adController.transform.position, adController.targetObjet.GetComponent<Aiming_Point>().Center,
                out RaycastHit hitInfo, adController.mask, QueryTriggerInteraction.Ignore))
            {
                if (hitInfo.distance <= adController.visionDistance)
                {
                    adController.ChangeState(typeof(StateAd_Following));
                }
            }

            if (Vector2.Distance(adController.spawnPoint, adController.transform.position) > 0.5) // дистанция не меньше
            {
                adController.SetDirectionAndRotation(new Vector2(adController.spawnPoint.x - adController.transform.position.x, 0).normalized.x);
                adController.rigidBody.velocity = new Vector3(adController.speed * adController.direction, adController.rigidBody.velocity.y, 0);
            }
            else
            {
                adController.SetDirectionAndRotation(-adController.direction);
                adController.ChangeState(typeof(StateAd_Idle));
            }
        }
        else
        {
            adController.ChangeState(typeof(StateAd_Idle));
        }
    }
}