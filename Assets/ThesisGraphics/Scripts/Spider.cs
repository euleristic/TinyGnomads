using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spider : MonoBehaviour
{
    //public Transform head_transform;
    //private Transform camera_transform;
    private Vector3 original_position;

    public NavMeshAgent agent;
    public float rotSpeed = 20.0f;

    //private const float DESTINATION_INTERVAL = 0.0f;
    //private float destination_timer;

    //private float camera_fov;

    //private float detection_radius = 15.0f;
    //private float camera_fov_vision_expansion = 10.0f;

    //public Animator animator;

    //private float slap_timer;
    //private const float SLAP_INTERVAL = 0.5f;
    //private bool waited_after_slap = false, can_start_timer = false, has_slaped = false;

    //stun
    //private float stun_timer;
    //private const float STUN_INTERVAL = 4.0f;
    //private bool stunned = false;

    //for reseting position
    public bool is_moving;
    public bool going_back;
    public bool hit_wall;

    //for Roaming
    private bool chosen_random_direction;
    private Vector3 randomPosition;

    //for staying stationary
    //public bool is_stationary = false;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        //camera_transform = Camera.main.transform;
        //camera_fov = Camera.main.fieldOfView;

        //destination_timer = 0.0f;

        is_moving = false;
        going_back = false;
        //chosen_random_direction = false;
        original_position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        InstantlyTurn(agent.destination);
        //Vector3 head_to_hand = transform.position - head_transform.position;

        //if (is_stationary)
        //{
        //    agent.SetDestination(transform.position);
        //}

        //if (IsReachable() == false && !going_back)
        //{
        //    //Debug.Log("not reachable");
        //    agent.SetDestination(transform.position);
        //    animator.SetBool("Out of range", true);
        //}
        //else
        //{
        //    animator.SetBool("Out of range", false);
        //}

        //if (head_to_hand.magnitude - agent.stoppingDistance <= 0.0f)
        //{
        //    agent.SetDestination(transform.position);
        //}

        //else if (head_to_hand.magnitude - agent.stoppingDistance > 0.0f && waited_after_slap == true && has_slaped == true)
        //{

        //}




        //for Roaming randomly

        if (is_moving == false && chosen_random_direction == false)
        {
            Vector3 randomDirection = Random.insideUnitSphere * 0.6f;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 0.6f, 1);
            randomPosition = hit.position;
            chosen_random_direction = true;
            print("chose direction");
        }
        if (chosen_random_direction == true)
        {
            is_moving = true;
            agent.SetDestination(randomPosition);
            print("going to destination");
            chosen_random_direction = false;
        }
        if (agent.remainingDistance <= 0.01f /*|| !IsReachable()*/)
        {
            //going_back = true;
            print("reached destination");
            is_moving = false;
        }
        //just for testing uncomment commented !IsReachable() above ^^^
        //if (hit_wall == true)
        //{
        //    //going_back = true;
        //    print("hit wall");
        //    is_moving = false;
        //    hit_wall = false;
        //}

        // for going to original position

        //if (going_back == true)
        //{
        //    if (agent.destination != original_position)
        //    {
        //        print("going back");
        //    }
        //    agent.SetDestination(original_position);
        //    if (agent.remainingDistance <= 0.001f)
        //    {
        //        is_moving = false;
        //        going_back = false;
        //        print("got back");
        //        //Debug.Log("not going back");
        //    }
        //}
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            is_moving = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            is_moving = false;
        }
    }

    //private bool IsReachable()
    //{
    //    NavMeshPath navMeshPath = new NavMeshPath();
    //    //Vector3 head_to_hand = transform.position - head_transform.position;
    //    //create path and check if it can be done
    //    // and check if navMeshAgent can reach its target

    //    if (agent.CalculatePath(agent.destination, navMeshPath) && navMeshPath.status == NavMeshPathStatus.PathComplete)
    //    {
    //        //if (head_to_hand.magnitude > detection_radius)
    //        //{
    //        //    //Debug.Log("Far Away");
    //        //    return false;
    //        //}
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}

    private void InstantlyTurn(Vector3 destination)
    {
        //When on target -> dont rotate!
        if ((destination - transform.position).magnitude < 0.1f) return;

        Vector3 direction = (destination - transform.position).normalized;
        Quaternion qDir = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, qDir, Time.deltaTime * rotSpeed);
    }

    public void resetDestination()
    {
        if (is_moving)
        {
            going_back = true;
            agent.SetDestination(original_position);
            //Debug.Log("going back = true");
            //has_moved = false;
        }
    }
}
