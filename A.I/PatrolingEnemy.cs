using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;


public class PatrolingEnemy : MonoBehaviour
{

    //public Transform[] points;
    public List<Transform> points = new List<Transform>();
    public GameObject PedRoute;

    public NavMeshAgent agent;
    public Animator anim;
    public float currentspeed;
    [Range(0, 10f)]
    public float movementSpeed;
    public Transform player;
    public float enemyFOV;


    //FSM
    private EnemyBaseState currentState;
    public readonly IdleState idlestate = new IdleState();
    public readonly PatrolState patrolstate = new PatrolState();
    public readonly AttackState attackstate = new AttackState();


    //DEBUG
    public bool attacksPlayer;
    void Start()
    {
        
        TransitionToState(idlestate);
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        // Disabling auto-braking allows for continuous movement
        // between points (ie, the agent doesn't slow down as it
        // approaches a destination point).
        agent.autoBraking = false;
        agent.speed = movementSpeed;
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GetRouteNodes();
    }


    public void TransitionToState(EnemyBaseState state)
    {
        // Set current state field to the param.
        currentState = state;
        //call enter state message as "this" current instance of the fsm class.
        currentState.EnterState(this);
    }


    void Update()
    {
        currentState.UpdateState(this);
        currentspeed = agent.speed;
        Debug.Log("Current State : " + currentState);
        //AnimationState();
        VisionCone(enemyFOV,10);
    }


    public void startroutines(string route)
    {
        StartCoroutine(route);
    }
    private IEnumerator WaitAtPoint()
    {
        agent.isStopped = true;
        agent.speed = 0;
        yield return new WaitForSeconds(Random.Range(3f, 5f));
        agent.speed = movementSpeed;
        agent.isStopped = false;
    }


    public void VisionCone(float visionAngle,float visionDist)
    {
        Vector3 direction = player.transform.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        if (angle < visionAngle * 0.5)
        {
            RaycastHit hit;
            if(Physics.Raycast(transform.position +transform.up,direction.normalized,out hit, visionDist))
            {
                if(hit.transform.tag == "Player")
                {
                    if(attacksPlayer)
                    Debug.Log("Player seen!");
                    TransitionToState(attackstate);
                }

            }

        }
        // DEBUG
        Debug.DrawRay(transform.position + transform.up,transform.forward,Color.green);
    }

    public void GetRouteNodes()
    {
        foreach(Transform child in PedRoute.transform)
        {
            points.Add(child.transform);
        }
    }

}
