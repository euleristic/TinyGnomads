using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spider : MonoBehaviour
{
    private Vector3 original_position;

    public NavMeshAgent agent;
    public float rotSpeed = 20.0f;

    //for reseting position
    public bool is_moving;
    public bool going_back;
    public bool hit_wall;

    //for Roaming
    private bool chosen_random_direction;
    private Vector3 randomPosition;

    //for staying stationary
    //public bool is_stationary = false;

    //timer
    public float timer;
    public float timer_interval;
    public bool started_timer;
    public bool boolean_to_change;
    public bool can_hit_again;

    private bool can_move;
    private float start_moving_after = 2.0f;
    private string action_to_do;

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
        started_timer = false;
        can_hit_again = true;
        can_move = false;
        //chosen_random_direction = false;
        original_position = transform.position;

        StartWallTimer(start_moving_after, "spider_movement");
    }

    // Update is called once per frame
    void Update()
    {
        if (started_timer)
        {
            UpdateTimer();
        }
        if (can_move)
        {
            Move();
        }
    }


    public void StartWallTimer(float interval, string action)
    {
        timer = 0;
        started_timer = true;

        timer_interval = interval;

        if(action == "spider_collision")
        {
            action_to_do = "spider_collision";
        }
        else if(action == "spider_movement")
        {
            action_to_do = "spider_movement";
        }
    }

    private void ChangeBoolean()
    {
        if (action_to_do == "spider_collision")
        {
            can_hit_again = true;
        }
        else if (action_to_do == "spider_movement")
        {
            can_move = true;
            //print("can move");
        }
    }

    private void resetWallCanHit()
    {
        can_hit_again = true;
    }

    private void UpdateTimer()
    {
        timer += Time.deltaTime;

        if (timer >= timer_interval)
        {
            started_timer = false;
            ChangeBoolean();
            timer = 0;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Wall"))
        {
            hit_wall = false;
            //print("turned from wall");
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if(can_hit_again)
        {
            if (other.gameObject.CompareTag("Wall"))
            {
                hit_wall = true;
                //print("hit wall");
                can_hit_again = false;
                StartWallTimer(Random.Range(0.2f,2.5f), "spider_collision");
            }
            if (other.gameObject.CompareTag("Hand Wall"))
            {
                hit_wall = true;
                print("hit hand");
                can_hit_again = false;
                StartWallTimer(Random.Range(0.1f, 0.1f), "spider_collision");
            }
        }  
    }

    private void Move()
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
            //print("chose direction");
        }
        if (chosen_random_direction == true)
        {
            is_moving = true;
            agent.SetDestination(randomPosition);
            //print("going to destination");
            chosen_random_direction = false;
        }
        if (agent.remainingDistance <= 0.01f /*|| !IsReachable()*/)
        {
            //going_back = true;
            //print("reached destination");
            is_moving = false;
        }
        //just for testing uncomment commented !IsReachable() above ^^^
        if (hit_wall == true)
        {
            //going_back = true;
            hit_wall = false;
            is_moving = false;
        }

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
